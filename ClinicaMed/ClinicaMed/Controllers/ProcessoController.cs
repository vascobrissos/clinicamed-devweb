using Microsoft.AspNetCore.Mvc;
using ClinicaMed.Models;
using System;
using System.Linq;
using ClinicaMed.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var processo = _context.Processo.Include(p => p.Examinando).Include(p => p.Requisitante).FirstOrDefault(p => p.IdPro == id);
            if (processo == null)
            {
                return NotFound();
            }

        
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
    }
}
