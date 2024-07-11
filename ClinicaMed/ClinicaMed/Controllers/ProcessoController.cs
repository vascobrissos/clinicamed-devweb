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
            var processos = _context.Processo.Include(p => p.Examinando).ToList();
            return View(processos);
        }

        // Visualizar detalhes de um processo específico
        public IActionResult Details(int? id)
        {
            var processo = _context.Processo.Include(p => p.Examinando).Include(p => p.Requisitante).Include(p => p.ListaProceColab).ThenInclude(pc => pc.Colaborador).FirstOrDefault(p => p.IdPro == id);
            if (processo == null)
            {
                return NotFound();
            }

            //Buscar médicos associados
            var medicos = _context.Colaborador.Where(c => _context.UserRoles.Any(nur => nur.UserId == c.UserId && nur.RoleId == "med")).
                Select(c => new
                {
                    c.IdCol,
                    NomeCompleto = c.Nome + " " + c.Apelido
                }).ToList();

            ViewData["Medicos"] = new SelectList(medicos, "IdCol", "NomeCompleto");


            return View(processo);
        }

        // Criar um processo
        public IActionResult Create()
        {
            var newProcesso = new Processo
            {
                IdInterno = Guid.NewGuid().ToString(), // Gera um ID interno único //ALTERAR PARA FORMATACAO ANO E ID
                DataCriacao = DateTime.Now,
                DataInicio = DateOnly.FromDateTime(DateTime.Now),
                DataTermino = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
                Estado = 1, // Estado inicial
            };

            _context.Processo.Add(newProcesso);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = newProcesso.IdPro });
        }

        //Associa médicos ao processo
        [HttpPost]
        public async Task<IActionResult> AssociateMedico(int processoId, int medicoId)
        {
            //Verificação se o médico selecionado já foi associado previamente
            bool medicoJaAssociado = _context.ProcessoColaborador.Any(pc => pc.ProcessoFK == processoId && pc.ColaboradorFK == medicoId);

            if (medicoJaAssociado)
            {
                ViewData["ErrorMessage"] = "Este médico já está associado ao processo.";
                var processo = await _context.Processo
                .Include(p => p.Examinando)
                .Include(p => p.Requisitante)
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
    }
}
