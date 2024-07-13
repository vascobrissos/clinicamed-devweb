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
    // O atributo [Authorize] assegura que apenas utilizadores autenticados podem aceder a este controlador
    [Authorize]
    public class ConsultaController : Controller
    {
        private readonly ApplicationDbContext _context; // Contexto da base de dados

        // Construtor do controlador, recebe o contexto da base de dados
        public ConsultaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Consulta
        public async Task<IActionResult> Index()
        {
            // Obter todas as consultas, incluindo dados de colaboradores e processos
            var consultas = _context.Consulta.Include(c => c.Colaborador).Include(c => c.Processo);
            return View(await consultas.ToListAsync()); // Retornar a lista de consultas para a view
        }

        // GET: Consulta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // Verifica se o ID é nulo
            {
                return NotFound(); // Retorna erro 404
            }

            // Procurar a consulta pelo ID
            var consulta = await _context.Consulta
                .Include(c => c.Colaborador)
                .Include(c => c.Processo)
                .FirstOrDefaultAsync(m => m.IdCon == id);
            if (consulta == null) // Verifica se a consulta existe
            {
                return NotFound(); // Retorna erro 404
            }

            return View(consulta); // Retornar a view com os detalhes da consulta
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

            // Criar opções para o dropdown de médicos
            var options = string.Join("", medicos.Select(m => $"<option value=\"{m.IdCol}\">{m.NomeCompleto}</option>"));

            ViewData["MedicosOptions"] = options; // Adicionar opções ao ViewData
            ViewData["ProcessoId"] = processoId; // Adicionar ID do processo ao ViewData

            var model = new Consulta(); // Criar um novo modelo de consulta
            return View(model); // Retornar a view para criar uma nova consulta
        }

        // POST: Consulta/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF
        public async Task<IActionResult> Create([Bind("DataConsulta,Observacoes,Estado")] Consulta consulta, int processoId, int colaboradorId)
        {
            // Carregar o processo associado ao processoId
            var processo = await _context.Processo.FindAsync(processoId);
            if (processo == null) // Verifica se o processo existe
            {
                return NotFound(); // Retorna erro 404
            }

            // Carregar o colaborador associado ao colaboradorId
            var colaborador = await _context.Colaborador.FindAsync(colaboradorId);
            if (colaborador == null) // Verifica se o colaborador existe
            {
                return NotFound(); // Retorna erro 404
            }

            // Associar processo e colaborador à consulta
            consulta.Processo = processo;
            consulta.Colaborador = colaborador;
            consulta.Estado = 1; // Definir estado como ativo

            if (ModelState.IsValid) // Verifica se o modelo é válido
            {
                _context.Add(consulta); // Adiciona a consulta ao contexto
                await _context.SaveChangesAsync(); // Salva as alterações na base de dados
                return RedirectToAction("Details", "Processo", new { id = consulta.ProcessoFK }); // Redireciona para os detalhes do processo
            }

            // Se o ModelState não for válido, retornar a lista de colaboradores
            var colaboradores = _context.ProcessoColaborador
                .Where(pc => pc.ProcessoFK == processoId)
                .Select(pc => pc.Colaborador)
                .ToList();

            ViewBag.Colaboradores = colaboradores; // Passa a lista de colaboradores para a view
            return View(consulta); // Retornar a view com o modelo da consulta
        }

        // GET: Consulta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) // Verifica se o ID é nulo
            {
                return NotFound(); // Retorna erro 404
            }

            var consulta = await _context.Consulta.FindAsync(id); // Procura a consulta pelo ID
            if (consulta == null) // Verifica se a consulta existe
            {
                return NotFound(); // Retorna erro 404
            }

            // Carrega os colaboradores e processos para o dropdown na view
            ViewData["ColaboradorFK"] = new SelectList(_context.Colaborador, "IdCol", "Apelido", consulta.ColaboradorFK);
            ViewData["ProcessoFK"] = new SelectList(_context.Processo, "IdPro", "IdPro", consulta.ProcessoFK);
            return View(consulta); // Retorna a view de edição com a consulta
        }

        // POST: Consulta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCon,DataConsulta,Observacoes,Estado,ColaboradorFK,ProcessoFK")] Consulta consulta)
        {
            if (id != consulta.IdCon) // Verifica se o ID da consulta corresponde ao ID fornecido
            {
                return NotFound(); // Retorna erro 404 se não corresponder
            }

            if (ModelState.IsValid) // Verifica se o estado do modelo é válido
            {
                try
                {
                    consulta.Estado = 1; // Definir estado como ativo
                    _context.Update(consulta); // Atualiza a consulta no contexto
                    await _context.SaveChangesAsync(); // Salvar alterações
                }
                catch (DbUpdateConcurrencyException) // Captura exceções de concorrência
                {
                    return NotFound(); // Retorna erro 404 se a consulta não existir
                }
                return RedirectToAction(nameof(Details), new { id = consulta.IdCon }); // Redirecionar para os detalhes da consulta
            }
            // Se o ModelState não for válido, carregar colaboradores e processos
            ViewData["ColaboradorFK"] = new SelectList(_context.Colaborador, "IdCol", "Apelido", consulta.ColaboradorFK);
            ViewData["ProcessoFK"] = new SelectList(_context.Processo, "IdPro", "IdPro", consulta.ProcessoFK);
            return View(consulta); // Retornar a view com o modelo da consulta se o estado não for válido
        }

        // POST: Consulta/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var consulta = await _context.Consulta.FindAsync(id); // Encontrar a consulta pelo ID
            if (consulta == null) // Verifica se a consulta existe
            {
                return NotFound(); // Retorna erro 404
            }

            _context.Consulta.Remove(consulta); // Remove a consulta do contexto
            await _context.SaveChangesAsync(); // Salva alterações na base de dados

            return RedirectToAction("Details", "Processo", new { id = consulta.ProcessoFK }); // Redirecionar para os detalhes do processo
        }

        // POST: TerminarConsulta
        [HttpPost]
        public async Task<IActionResult> TerminarConsulta(int id)
        {
            var consulta = await _context.Consulta.FindAsync(id); // Encontrar a consulta pelo ID
            if (consulta == null) // Verifica se a consulta existe
            {
                return NotFound(); // Retorna erro 404
            }

            consulta.Estado = 0; // Definir estado como terminada
            _context.Update(consulta); // Atualiza a consulta no contexto
            await _context.SaveChangesAsync(); // Salvar alterações

            return RedirectToAction(nameof(Details), new { id = consulta.IdCon }); // Redirecionar para os detalhes da consulta
        }

        // POST: AtivarConsulta
        [HttpPost]
        public async Task<IActionResult> AtivarConsulta(int id)
        {
            var consulta = await _context.Consulta.FindAsync(id); // Encontrar a consulta pelo ID
            if (consulta == null) // Verifica se a consulta existe
            {
                return NotFound(); // Retorna erro 404
            }

            consulta.Estado = 1; // Definir estado como ativo
            _context.Update(consulta); // Atualiza a consulta no contexto
            await _context.SaveChangesAsync(); // Salvar alterações

            return RedirectToAction(nameof(Details), new { id = consulta.IdCon }); // Redirecionar para os detalhes da consulta
        }
    }
}
