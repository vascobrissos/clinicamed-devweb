using Microsoft.AspNetCore.Mvc;
using ClinicaMed.Models;
using System;
using System.Linq;
using ClinicaMed.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Security.Claims;

namespace ClinicaMed.Controllers
{
    [Authorize]
    public class ProcessoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listar todos os processos
        public IActionResult Index()
        {
            IQueryable<Processo> processos;

            if (User.IsInRole("Administrador") || User.IsInRole("Administrativo"))
            {
                // Administradores podem ver todos os processos
                processos = _context.Processo.Include(p => p.Examinandos);
            }
            else if (User.IsInRole("Medico"))
            {
                // Médicos só veem os processos associados a eles
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Obter ID do colaborador

                processos = from p in _context.Processo
                            join pc in _context.ProcessoColaborador on p.IdPro equals pc.ProcessoFK
                            join c in _context.Colaborador on pc.ColaboradorFK equals c.IdCol
                            where c.UserId == userId
                            select p;
            }
            else
            {
                // Para outros roles, retornar uma lista vazia ou uma mensagem de acesso negado
                processos = Enumerable.Empty<Processo>().AsQueryable();
            }

            // Ordenar a lista de processos pelo IdInterno
            var orderedProcessos = processos.OrderBy(p => p.IdInterno).ToList();

            return View(orderedProcessos); // Retorna a view com a lista ordenada de processos
        }

        // Visualizar detalhes de um processo específico
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // Verifica se o ID é nulo
            {
                return NotFound(); // Retorna erro
            }

            var processo = await _context.Processo
                .Include(p => p.Examinandos)
                .Include(p => p.Requisitantes)
                .Include(p => p.ListaProceColab)
                    .ThenInclude(pc => pc.Colaborador)
                .Include(p => p.ListaReceita)
                .Include(p => p.ListaConsulta)
                .FirstOrDefaultAsync(p => p.IdPro == id); // Procura o processo pelo ID

            if (processo == null) // Verifica se o processo existe
            {
                return NotFound(); // Retorna erro
            }

            var medicos = await _context.Colaborador
                .Where(c => _context.UserRoles.Any(nur => nur.UserId == c.UserId && nur.RoleId == "med")) // Filtra apenas médicos
                .Select(c => new
                {
                    c.IdCol,
                    NomeCompleto = c.Nome + " " + c.Apelido // Cria um nome completo
                }).ToListAsync();

            ViewData["Medicos"] = new SelectList(medicos, "IdCol", "NomeCompleto"); // Passa a lista de médicos para a view

            return View(processo); // Retorna a view com os detalhes do processo
        }

        // Criar um novo processo
        public IActionResult Create()
        {
            var newProcesso = new Processo
            {
                DataCriacao = DateTime.Now,
                DataInicio = DateOnly.FromDateTime(DateTime.Now),
                Estado = 1 // Estado inicial
            };

            _context.Processo.Add(newProcesso); // Adiciona o novo processo ao contexto
            _context.SaveChanges(); // Guarda as alterações

            var currentYear = DateTime.Now.ToString("yy");
            newProcesso.IdInterno = $"CM{currentYear}-{newProcesso.IdPro}"; // Gera um ID interno

            _context.Processo.Update(newProcesso); // Atualiza o processo com o ID interno
            _context.SaveChanges(); // Guarda as alterações

            return RedirectToAction(nameof(Details), new { id = newProcesso.IdPro }); // Redireciona para os detalhes do novo processo
        }

        // Associar médicos ao processo
        [HttpPost]
        public async Task<IActionResult> AssociateMedico(int processoId, int medicoId)
        {
            // Verifica se o médico já está associado ao processo
            bool medicoJaAssociado = _context.ProcessoColaborador.Any(pc => pc.ProcessoFK == processoId && pc.ColaboradorFK == medicoId);

            if (medicoJaAssociado)
            {
                ViewData["ErrorMessage"] = "Este médico já está associado ao processo.";
                var processo = await _context.Processo
                    .Include(p => p.Examinandos)
                    .Include(p => p.Requisitantes)
                    .Include(p => p.ListaProceColab)
                        .ThenInclude(pc => pc.Colaborador)
                    .FirstOrDefaultAsync(m => m.IdPro == processoId);

                var medicos = _context.Colaborador
                    .Where(c => _context.UserRoles.Any(nur => nur.UserId == c.UserId && nur.RoleId == "med"))
                    .Select(c => new
                    {
                        c.IdCol,
                        NomeCompleto = c.Nome + " " + c.Apelido
                    })
                    .ToList();

                ViewData["Medicos"] = new SelectList(medicos, "IdCol", "NomeCompleto");

                return View("Details", processo); // Retorna a view de detalhes com mensagem de erro
            }

            var processoColaborador = new ProcessoColaborador
            {
                ProcessoFK = processoId,
                ColaboradorFK = medicoId,
                DataAtribuicao = DateTime.Now,
                DataRemocao = DateTime.MaxValue // Data de remoção definida para um valor máximo
            };

            _context.ProcessoColaborador.Add(processoColaborador); // Adiciona a associação ao contexto
            await _context.SaveChangesAsync(); // Guarda as alterações

            return RedirectToAction(nameof(Details), new { id = processoId }); // Redireciona para os detalhes do processo
        }

        // Remover médico associado ao processo
        [HttpPost]
        public async Task<IActionResult> RemoveMedico(int processoId, int medicoId)
        {
            var processoColaborador = await _context.ProcessoColaborador
                .FirstOrDefaultAsync(pc => pc.ProcessoFK == processoId && pc.ColaboradorFK == medicoId);

            if (processoColaborador != null)
            {
                _context.ProcessoColaborador.Remove(processoColaborador); // Remove a associação do contexto
                await _context.SaveChangesAsync(); // Salva as alterações
            }

            return RedirectToAction(nameof(Details), new { id = processoId }); // Redireciona para os detalhes do processo
        }

        // Terminar um processo
        [HttpPost]
        public async Task<IActionResult> Terminate(int id)
        {
            var processo = await _context.Processo.FindAsync(id);
            if (processo == null)
            {
                return NotFound(); // Retorna erro se não existir
            }

            processo.Estado = 0; // Define o estado como terminado
            processo.DataTermino = DateOnly.FromDateTime(DateTime.Now); // Define a data de término

            _context.Processo.Update(processo); // Atualiza o processo no contexto
            await _context.SaveChangesAsync(); // Guarda as alterações

            return RedirectToAction(nameof(Details), new { id = processo.IdPro }); // Redireciona para os detalhes do processo
        }

        // Re-ativar um processo
        [HttpPost]
        public async Task<IActionResult> Reactivate(int id)
        {
            var processo = await _context.Processo.FindAsync(id);
            if (processo == null)
            {
                return NotFound(); // Retorna erro se não existir
            }

            processo.Estado = 1; // Define o estado como ativo
            processo.DataTermino = null; // Limpa a data de término

            _context.Processo.Update(processo); // Atualiza o processo no contexto
            await _context.SaveChangesAsync(); // Guarda as alterações

            return RedirectToAction(nameof(Details), new { id = processo.IdPro }); // Redireciona para os detalhes do processo
        }
    }
}
