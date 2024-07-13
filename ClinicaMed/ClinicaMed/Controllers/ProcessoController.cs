using Microsoft.AspNetCore.Mvc;
using ClinicaMed.Models;
using System;
using System.Linq;
using ClinicaMed.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

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
            var processos = _context.Processo.Include(p => p.Examinandos).ToList();
            return View(processos);
        }

        // Visualizar detalhes de um processo específico
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processo
                .Include(p => p.Examinandos)
                .Include(p => p.Requisitantes)
                .Include(p => p.ListaProceColab)
                    .ThenInclude(pc => pc.Colaborador)
                .Include(p => p.ListaReceita)
                .Include(p => p.ListaConsulta)
                .FirstOrDefaultAsync(p => p.IdPro == id);

            if (processo == null)
            {
                return NotFound();
            }

            var medicos = await _context.Colaborador
                .Where(c => _context.UserRoles.Any(nur => nur.UserId == c.UserId && nur.RoleId == "med"))
                .Select(c => new
                {
                    c.IdCol,
                    NomeCompleto = c.Nome + " " + c.Apelido
                }).ToListAsync();

            ViewData["Medicos"] = new SelectList(medicos, "IdCol", "NomeCompleto");

            return View(processo);
        }

        // Criar um processo
        public IActionResult Create()
        {
            var newProcesso = new Processo
            {
                DataCriacao = DateTime.Now,
                DataInicio = DateOnly.FromDateTime(DateTime.Now),
                Estado = 1 // Estado inicial
            };

            _context.Processo.Add(newProcesso);
            _context.SaveChanges();

            var currentYear = DateTime.Now.ToString("yy");
            newProcesso.IdInterno = $"CM{currentYear}-{newProcesso.IdPro}";

            _context.Processo.Update(newProcesso);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = newProcesso.IdPro });
        }

        // Associar médicos ao processo
        [HttpPost]
        public async Task<IActionResult> AssociateMedico(int processoId, int medicoId)
        {
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

                return View("Details", processo);
            }

            var processoColaborador = new ProcessoColaborador
            {
                ProcessoFK = processoId,
                ColaboradorFK = medicoId,
                DataAtribuicao = DateTime.Now,
                DataRemocao = DateTime.MaxValue
            };

            _context.ProcessoColaborador.Add(processoColaborador);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = processoId });
        }

        // Remover médico associado ao processo
        [HttpPost]
        public async Task<IActionResult> RemoveMedico(int processoId, int medicoId)
        {
            var processoColaborador = await _context.ProcessoColaborador
                .FirstOrDefaultAsync(pc => pc.ProcessoFK == processoId && pc.ColaboradorFK == medicoId);

            if (processoColaborador != null)
            {
                _context.ProcessoColaborador.Remove(processoColaborador);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = processoId });
        }

        // Terminar um processo
        [HttpPost]
        public async Task<IActionResult> Terminate(int id)
        {
            var processo = await _context.Processo.FindAsync(id);
            if (processo == null)
            {
                return NotFound();
            }

            processo.Estado = 0;
            processo.DataTermino = DateOnly.FromDateTime(DateTime.Now);

            _context.Processo.Update(processo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = processo.IdPro });
        }

        // Re-ativar um processo
        [HttpPost]
        public async Task<IActionResult> Reactivate(int id)
        {
            var processo = await _context.Processo.FindAsync(id);
            if (processo == null)
            {
                return NotFound();
            }

            processo.Estado = 1;
            processo.DataTermino = null;

            _context.Processo.Update(processo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = processo.IdPro });
        }
    }
}
