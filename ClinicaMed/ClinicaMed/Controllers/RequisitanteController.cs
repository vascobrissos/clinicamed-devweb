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
            ViewBag.processoId = processoId;

            var model = new Requisitante();
            return View(model);
        }

        // POST: Requisitante/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdReq,Nome,Apelido,Telemovel,Email,Sexo,Pais,Morada,CodigoPostal,Localidade,Nacionalidade,Nif")] Requisitante requisitante, int? processoId)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requisitante);
                await _context.SaveChangesAsync();

                if (processoId.HasValue)
                {
                    var processo = await _context.Processo.FindAsync(processoId.Value);
                    if (processo != null)
                    {
                        processo.RequisitanteIdReq= requisitante.IdReq;
                        _context.Update(processo);
                        await _context.SaveChangesAsync();
                        // Caso exista processo, retorna para os detalhes do mesmo 
                        return RedirectToAction("Details", "Processo", new { id = processoId.Value });
                    }
                }
                return RedirectToAction(nameof(Index));
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReq,Nome,Apelido,Telemovel,Email,Sexo,Pais,Morada,CodigoPostal,Localidade,Nacionalidade,Nif")] Requisitante requisitante)
        {
            if (id != requisitante.IdReq)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
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

        [HttpGet]
        public async Task<IActionResult> AssociarProc(int id, int processoId)
        {
            //Buscar o processo recebido por asp-route-id de forma asincrona onde o processo corresponda ao passado no mesmo
            var processo = await _context.Processo.Include(p => p.Requisitante).FirstOrDefaultAsync(p => p.IdPro == processoId);
            if (processo == null)
            {
                return NotFound();
            }

            //guarda o id do examinando no processo associado
            processo.RequisitanteIdReq = id;
            _context.Update(processo);
            await _context.SaveChangesAsync();


            //BUscar o examinando pelo id fornecido
            var requisitante = await _context.Requisitante.FirstOrDefaultAsync(e => e.IdReq == id);
            if (requisitante == null)
            {
                return NotFound();
            }

            //guarda o processo na lista de processos do examinando
            requisitante.ListaProcesso.Add(processo);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Processo", new { id = processoId });
        }
    }
}
