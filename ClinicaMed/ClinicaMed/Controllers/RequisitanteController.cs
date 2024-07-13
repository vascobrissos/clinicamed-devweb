using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicaMed.Data;
using ClinicaMed.Models;
using Microsoft.AspNetCore.Authorization;

namespace ClinicaMed.Controllers
{
    [Authorize]
    public class RequisitanteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequisitanteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Requisitante
        public async Task<IActionResult> Index(int? processoId)
        {
            ViewData["ProcessoId"] = processoId;
            return View(await _context.Requisitante.ToListAsync());
        }

        // GET: Requisitante/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisitante = await _context.Requisitante
                .FirstOrDefaultAsync(m => m.IdReq == id);
            if (requisitante == null)
            {
                return NotFound();
            }

            return View(requisitante);
        }

        // GET: Requisitante/Create
        public IActionResult Create(int? processoId)
        {
            ViewBag.ProcessoId = processoId;
            return View(new Requisitante());
        }

        // POST: Requisitante/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdReq,Nome,Apelido,Telemovel,Email,Sexo,Pais,Morada,CodigoPostal,Localidade,Nacionalidade,Nif")] Requisitante requisitante, int? processoId)
        {
            if (ModelState.IsValid)
            {
                requisitante.ProcessoId = processoId.HasValue ? processoId.Value : default;

                _context.Add(requisitante);
                await _context.SaveChangesAsync();

                // Redireciona para os detalhes do processo
                return RedirectToAction("Details", "Processo", new { id = processoId });
            }
            return View(requisitante);
        }

        // GET: Requisitante/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisitante = await _context.Requisitante.FindAsync(id);
            if (requisitante == null)
            {
                return NotFound();
            }
            return View(requisitante);
        }

        // POST: Requisitante/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReq,Nome,Apelido,Telemovel,Email,Sexo,Pais,Morada,CodigoPostal,Localidade,Nacionalidade,Nif")] Requisitante requisitante, int? processoId)
        {
            if (id != requisitante.IdReq)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    requisitante.ProcessoId = (int)processoId;

                    _context.Update(requisitante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequisitanteExists(requisitante.IdReq))
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
            return View(requisitante);
        }

        // GET: Requisitante/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisitante = await _context.Requisitante
                .FirstOrDefaultAsync(m => m.IdReq == id);
            if (requisitante == null)
            {
                return NotFound();
            }

            return View(requisitante);
        }

        // POST: Requisitante/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requisitante = await _context.Requisitante.FindAsync(id);
            if (requisitante != null)
            {
                _context.Requisitante.Remove(requisitante);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequisitanteExists(int id)
        {
            return _context.Requisitante.Any(e => e.IdReq == id);
        }
    }
}
