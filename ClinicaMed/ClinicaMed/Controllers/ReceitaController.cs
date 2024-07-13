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
            // Buscar médicos associados ao processo
            var medicos = _context.ProcessoColaborador
                .Where(pc => pc.ProcessoFK == processoId)
                .Select(pc => pc.Colaborador)
                .Where(c => _context.UserRoles.Any(nur => nur.UserId == c.UserId && nur.RoleId == "med"))
                .Select(c => new
                {
                    c.IdCol,
                    NomeCompleto = c.Nome + " " + c.Apelido
                }).ToList();

            var options = string.Join("", medicos.Select(m => $"<option value=\"{m.IdCol}\">{m.NomeCompleto}</option>"));

            ViewData["MedicosOptions"] = options;
            ViewData["ProcessoId"] = processoId;

            var model = new Receita();
            return View(model);
        }


        // POST: Receita/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumReceita,Notas,DataReceita,Estado")] Receita receita, int processoId, int colaboradorId)
        {
            // Carregar o processo associado ao processoId
            var processo = await _context.Processo.FindAsync(processoId);
            if (processo == null)
            {
                return NotFound();
            }

            // Carregar o colaborador associado ao colaboradorId
            var colaborador = await _context.Colaborador.FindAsync(colaboradorId);
            if (colaborador == null)
            {
                return NotFound();
            }

            // Associar o processo e o colaborador ao objeto Receita
            receita.Processo = processo;
            receita.Colaborador = colaborador; 

            // Verificar a validade do ModelState agora que as propriedades estão preenchidas corretamente
            if (ModelState.IsValid)
            {
                _context.Add(receita);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Processo", new { id = receita.ProcessoFK });
            }

            // Se o ModelState não for válido, carregar a lista de colaboradores associados ao processo
            var medicosAssociados = _context.Processo
                .Include(p => p.ListaProceColab)
                .ThenInclude(pc => pc.Colaborador)
                .Where(p => p.IdPro == processoId)
                .SelectMany(p => p.ListaProceColab.Select(pc => pc.Colaborador))
                .ToList();

            ViewBag.ColaboradorFK = new SelectList(medicosAssociados.Select(c => new { c.IdCol, NomeCompleto = c.Nome + " " + c.Apelido }), "IdCol", "NomeCompleto", colaboradorId);
            ViewData["ProcessoId"] = processoId;

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
