using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaMed.Data;
using ClinicaMed.Models;

namespace ClinicaMed.Controllers
{
    public class ReceitaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReceitaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Receita
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Receita.Include(r => r.Colaborador).Include(r => r.Processo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Receita/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receita
                .Include(r => r.Colaborador)
                .Include(r => r.Processo)
                .FirstOrDefaultAsync(m => m.IdRec == id);
            if (receita == null)
            {
                return NotFound();
            }

            return View(receita);
        }

        // GET: Receita/Create
        public IActionResult Create(int processoId)
        {
            ViewData["Title"] = "Criar Receita";
            ViewData["ColaboradorFK"] = new SelectList(_context.Colaborador, "IdCol", "Apelido");
            ViewData["ProcessoId"] = processoId;

            return View();
        }

        // POST: Receita/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRec,NumReceita,Notas,DataReceita,Estado,ProcessoFK,ColaboradorFK")] Receita receita)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receita);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Processo", new { id = receita.ProcessoFK });
            }

            ViewData["ColaboradorFK"] = new SelectList(_context.Colaborador, "IdCol", "Apelido", receita.ColaboradorFK);
            ViewData["ProcessoId"] = receita.ProcessoFK;

            return View(receita);
        }


        // GET: Receita/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receita.FindAsync(id);
            if (receita == null)
            {
                return NotFound();
            }
            ViewData["ColaboradorFK"] = new SelectList(_context.Colaborador, "IdCol", "Apelido", receita.ColaboradorFK);
            ViewData["ProcessoFK"] = new SelectList(_context.Processo, "IdPro", "IdPro", receita.ProcessoFK);
            return View(receita);
        }

        // POST: Receita/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRec,NumReceita,Notas,DataReceita,Estado,ProcessoFK,ColaboradorFK")] Receita receita)
        {
            if (id != receita.IdRec)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceitaExists(receita.IdRec))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColaboradorFK"] = new SelectList(_context.Colaborador, "IdCol", "Apelido", receita.ColaboradorFK);
            ViewData["ProcessoFK"] = new SelectList(_context.Processo, "IdPro", "IdPro", receita.ProcessoFK);
            return View(receita);
        }

        // GET: Receita/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receita
                .Include(r => r.Colaborador)
                .Include(r => r.Processo)
                .FirstOrDefaultAsync(m => m.IdRec == id);
            if (receita == null)
            {
                return NotFound();
            }

            return View(receita);
        }

        // POST: Receita/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receita = await _context.Receita.FindAsync(id);
            if (receita != null)
            {
                _context.Receita.Remove(receita);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceitaExists(int id)
        {
            return _context.Receita.Any(e => e.IdRec == id);
        }
    }
}
