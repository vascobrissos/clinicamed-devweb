using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaMed.Data;
using ClinicaMed.Models;
using Microsoft.AspNetCore.Authorization;

namespace ClinicaMed.Controllers
{
    [Authorize]
    public class ExaminandoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExaminandoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Examinando
        public async Task<IActionResult> Index(int? processoId)
        {
            ViewData["ProcessoId"] = processoId;
            return View(await _context.Examinando.ToListAsync());
        }

        // GET: Examinando/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examinando = await _context.Examinando
                .FirstOrDefaultAsync(m => m.IdExa == id);
            if (examinando == null)
            {
                return NotFound();
            }

            return View(examinando);
        }

        // GET: Examinando/Create
        public IActionResult Create(int? processoId)
        {
            ViewBag.ProcessoId = processoId;
            return View(new Examinando());
        }

        // POST: Examinando/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdExa,Nome,Apelido,Telemovel,Email,Sexo,Antecedentes,DataNascimento,Profissao,Pais,Morada,CodigoPostal,Localidade,Nacionalidade,NumUtente,CartaoCidadao,ValidadeCC,Nif,Seguradora,NumeroSeguro,FiliacaoMae,FiliacaoPai")] Examinando examinando, int? processoId)
        {
            if (ModelState.IsValid)
            {
                examinando.ProcessoId = (int)processoId;

                _context.Add(examinando);
                await _context.SaveChangesAsync();

                // Redireciona para os detalhes do processo
                return RedirectToAction("Details", "Processo", new { id = processoId });
            }
            return View(examinando);
        }

        // GET: Examinando/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examinando = await _context.Examinando.FindAsync(id);
            if (examinando == null)
            {
                return NotFound();
            }
            return View(examinando);
        }

        // POST: Examinando/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdExa,Nome,Apelido,Telemovel,Email,Sexo,Antecedentes,DataNascimento,Profissao,Pais,Morada,CodigoPostal,Localidade,Nacionalidade,NumUtente,CartaoCidadao,ValidadeCC,Nif,Seguradora,NumeroSeguro,FiliacaoMae,FiliacaoPai")] Examinando examinando, int? processoId)
        {
            if (id != examinando.IdExa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    examinando.ProcessoId = (int)processoId;

                    _context.Update(examinando);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExaminandoExists(examinando.IdExa))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Processo", new { id = processoId });
            }
            return View(examinando);
        }

        // GET: Examinando/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examinando = await _context.Examinando
                .FirstOrDefaultAsync(m => m.IdExa == id);
            if (examinando == null)
            {
                return NotFound();
            }

            return View(examinando);
        }

        // POST: Examinando/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var examinando = await _context.Examinando.FindAsync(id);
            if (examinando != null)
            {
                _context.Examinando.Remove(examinando);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExaminandoExists(int id)
        {
            return _context.Examinando.Any(e => e.IdExa == id);
        }
    }
}
