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
    [Authorize] // Garante que apenas utilizadores autenticados podem aceder a este controlador
    public class ReceitaController : Controller
    {
        private readonly ApplicationDbContext _context; // Contexto da base de dados

        public ReceitaController(ApplicationDbContext context)
        {
            _context = context; // Inicializa o contexto
        }

        // GET: Receita
        public async Task<IActionResult> Index()
        {
            // Carrega todas as receitas incluindo colaboradores e processos associados
            var applicationDbContext = _context.Receita.Include(r => r.Colaborador).Include(r => r.Processo);
            return View(await applicationDbContext.ToListAsync()); // Retorna a lista de receitas à view
        }

        // GET: Receita/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // Verifica se o ID é nulo
            {
                return NotFound(); // Retorna erro 404
            }

            // Busca a receita pelo ID, incluindo colaborador e processo
            var receita = await _context.Receita
                .Include(r => r.Colaborador)
                .Include(r => r.Processo)
                .FirstOrDefaultAsync(m => m.IdRec == id);
            if (receita == null) // Verifica se a receita existe
            {
                return NotFound(); // Retorna erro 404
            }

            return View(receita); // Retorna a view com os detalhes da receita
        }

        // GET: Receita/Create
        public IActionResult Create(int processoId)
        {
            // Busca médicos associados ao processo
            var medicos = _context.ProcessoColaborador
                .Where(pc => pc.ProcessoFK == processoId)
                .Select(pc => pc.Colaborador)
                .Where(c => _context.UserRoles.Any(nur => nur.UserId == c.UserId && nur.RoleId == "med"))
                .Select(c => new
                {
                    c.IdCol,
                    NomeCompleto = c.Nome + " " + c.Apelido
                }).ToList();

            // Cria opções de médicos para seleção
            var options = string.Join("", medicos.Select(m => $"<option value=\"{m.IdCol}\">{m.NomeCompleto}</option>"));

            ViewData["MedicosOptions"] = options; // Passa opções de médicos para a view
            ViewData["ProcessoId"] = processoId; // Passa ID do processo

            var model = new Receita(); // Cria uma nova instância de Receita
            return View(model); // Retorna a view de criação de receita
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
                return NotFound(); // Retorna erro 404 se o processo não existir
            }

            // Carregar o colaborador associado ao colaboradorId
            var colaborador = await _context.Colaborador.FindAsync(colaboradorId);
            if (colaborador == null)
            {
                return NotFound(); // Retorna erro 404 se o colaborador não existir
            }

            // Associar o processo e o colaborador à nova receita
            receita.Processo = processo;
            receita.Colaborador = colaborador;

            if (ModelState.IsValid) // Verifica a validade do modelo
            {
                _context.Add(receita); // Adiciona a nova receita ao contexto
                await _context.SaveChangesAsync(); // Salva as alterações
                return RedirectToAction("Details", "Processo", new { id = receita.ProcessoFK }); // Redireciona para os detalhes do processo
            }

            // Se o modelo não for válido, carrega a lista de colaboradores associados ao processo
            var medicosAssociados = _context.Processo
                .Include(p => p.ListaProceColab)
                .ThenInclude(pc => pc.Colaborador)
                .Where(p => p.IdPro == processoId)
                .SelectMany(p => p.ListaProceColab.Select(pc => pc.Colaborador))
                .ToList();

            ViewBag.ColaboradorFK = new SelectList(medicosAssociados.Select(c => new { c.IdCol, NomeCompleto = c.Nome + " " + c.Apelido }), "IdCol", "NomeCompleto", colaboradorId);
            ViewData["ProcessoId"] = processoId;

            return View(receita); // Retorna a view com a receita não válida
        }

        // GET: Receita/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Retorna erro 404
            }

            var receita = await _context.Receita.FindAsync(id);
            if (receita == null)
            {
                return NotFound(); // Retorna erro 404 se a receita não existir
            }
            // Prepara listas de seleção para a edição
            ViewData["ColaboradorFK"] = new SelectList(_context.Colaborador, "IdCol", "Apelido", receita.ColaboradorFK);
            ViewData["ProcessoFK"] = new SelectList(_context.Processo, "IdPro", "IdPro", receita.ProcessoFK);
            return View(receita); // Retorna a view de edição da receita
        }

        // POST: Receita/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRec,NumReceita,Notas,DataReceita,Estado,ProcessoFK,ColaboradorFK")] Receita receita)
        {
            if (id != receita.IdRec) // Verifica se o ID da receita corresponde
            {
                return NotFound(); // Retorna erro 404 se não corresponder
            }

            if (ModelState.IsValid) // Verifica se o modelo é válido
            {
                try
                {
                    _context.Update(receita); // Atualiza a receita no contexto
                    await _context.SaveChangesAsync(); // Salva as alterações
                }
                catch (DbUpdateConcurrencyException) // Captura exceção de concorrência
                {
                    return NotFound(); // Retorna erro 404 se a receita não existir
                }
                return RedirectToAction(nameof(Details), new { id = receita.IdRec }); // Redireciona para os detalhes da receita
            }
            // Se o modelo não for válido, prepara listas de seleção
            ViewData["ColaboradorFK"] = new SelectList(_context.Colaborador, "IdCol", "Apelido", receita.ColaboradorFK);
            ViewData["ProcessoFK"] = new SelectList(_context.Processo, "IdPro", "IdPro", receita.ProcessoFK);
            return View(receita); // Retorna a view com a receita não válida
        }

        // POST: Receita/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var receita = await _context.Receita.FindAsync(id);
            if (receita == null)
            {
                return NotFound(); // Retorna erro 404 se a receita não existir
            }

            _context.Receita.Remove(receita); // Remove a receita do contexto
            await _context.SaveChangesAsync(); // Salva as alterações

            return RedirectToAction("Details", "Processo", new { id = receita.ProcessoFK }); // Redireciona para os detalhes do processo após a exclusão
        }
    }
}
