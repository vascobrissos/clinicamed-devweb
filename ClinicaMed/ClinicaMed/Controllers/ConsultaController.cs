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
    public class ConsultaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConsultaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Consulta
        public async Task<IActionResult> Index()
        {
            var consultas = _context.Consulta.Include(c => c.Colaborador).Include(c => c.Processo);
            return View(await consultas.ToListAsync());
        }

        // GET: Consulta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consulta
                .Include(c => c.Colaborador)
                .Include(c => c.Processo)
                .FirstOrDefaultAsync(m => m.IdCon == id);
            if (consulta == null)
            {
                return NotFound();
            }

            return View(consulta);
        }

        // GET: Consulta/Create
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

            var model = new Consulta();
            return View(model);
        }

        // POST: Consulta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataConsulta,Observacoes,Estado")] Consulta consulta, int processoId, int colaboradorId)
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

            consulta.Processo = processo;
            consulta.Colaborador = colaborador;
            consulta.Estado = 1;

            if (ModelState.IsValid)
            {
                _context.Add(consulta);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Processo", new { id = consulta.ProcessoFK });
            }

            // Se o ModelState não for válido, retornar a lista de colaboradores
            var colaboradores = _context.ProcessoColaborador
                .Where(pc => pc.ProcessoFK == processoId)
                .Select(pc => pc.Colaborador)
                .ToList();

            ViewBag.Colaboradores = colaboradores;
            return View(consulta);
        }

        // GET: Consulta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consulta.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }
            ViewData["ColaboradorFK"] = new SelectList(_context.Colaborador, "IdCol", "Apelido", consulta.ColaboradorFK);
            ViewData["ProcessoFK"] = new SelectList(_context.Processo, "IdPro", "IdPro", consulta.ProcessoFK);
            return View(consulta);
        }

        // POST: Consulta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCon,DataConsulta,Observacoes,Estado,ColaboradorFK,ProcessoFK")] Consulta consulta)
        {
            if (id != consulta.IdCon)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    consulta.Estado = 1;
                    _context.Update(consulta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Details), new { id = consulta.IdCon });
            }
            ViewData["ColaboradorFK"] = new SelectList(_context.Colaborador, "IdCol", "Apelido", consulta.ColaboradorFK);
            ViewData["ProcessoFK"] = new SelectList(_context.Processo, "IdPro", "IdPro", consulta.ProcessoFK);
            return View(consulta);
        }

        // POST: Consulta/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var consulta = await _context.Consulta.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }

            _context.Consulta.Remove(consulta);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Processo", new { id = consulta.ProcessoFK });
        }

        [HttpPost]
        public async Task<IActionResult> TerminarConsulta(int id)
        {
            var consulta = await _context.Consulta.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }

            consulta.Estado = 0; // Terminada
            _context.Update(consulta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = consulta.IdCon });
        }

        [HttpPost]
        public async Task<IActionResult> AtivarConsulta(int id)
        {
            var consulta = await _context.Consulta.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }

            consulta.Estado = 1; // Ativa
            _context.Update(consulta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = consulta.IdCon });
        }
    }
}
