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
    public class ExaminandoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExaminandoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Examinando
        public async Task<IActionResult> Index()
        {
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
            ViewBag.processoId = processoId;

           var model = new Examinando();
           return View(model);
        }

        // POST: Examinando/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdExa,Nome,Apelido,Telemovel,Email,Sexo,Antecedentes,DataNascimento,Profissao,Pais,Morada,CodigoPostal,Localidade,Nacionalidade,NumUtente,CartaoCidadao,ValidadeCC,Nif,Seguradora,NumeroSeguro,FiliacaoMae,FiliacaoPai")] Examinando examinando, int? processoId)
        {
            if (ModelState.IsValid)
            {
                _context.Add(examinando);
                await _context.SaveChangesAsync();

                if (processoId != null)
                {
                    var processo = await _context.Processo.FindAsync(processoId.Value);
                    if (processo != null)
                    {
                        processo.ExaminandoIdExa = examinando.IdExa;
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdExa,Nome,Apelido,Telemovel,Email,Sexo,Antecedentes,DataNascimento,Profissao,Pais,Morada,CodigoPostal,Localidade,Nacionalidade,NumUtente,CartaoCidadao,ValidadeCC,Nif,Seguradora,NumeroSeguro,FiliacaoMae,FiliacaoPai")] Examinando examinando)
        {
            if (id != examinando.IdExa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
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
