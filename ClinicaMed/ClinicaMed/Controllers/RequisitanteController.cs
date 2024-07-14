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
            ViewData["ProcessoId"] = processoId; // Passa o ID do processo para a view
            return View(await _context.Requisitante.ToListAsync()); // Retorna a lista de requisitantes à view
        }

        // GET: Requisitante/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // Verifica se o ID é nulo
            {
                return NotFound(); // Retorna erro
            }

            // Procura o requisitante pelo ID
            var requisitante = await _context.Requisitante
                .FirstOrDefaultAsync(m => m.IdReq == id);
            if (requisitante == null) // Verifica se o requisitante existe
            {
                return NotFound(); // Retorna erro
            }

            return View(requisitante); // Retorna a view com os detalhes do requisitante
        }

        // GET: Requisitante/Create
        public IActionResult Create(int? processoId)
        {
            ViewBag.ProcessoId = processoId; // Passa o ID do processo para a view
            return View(new Requisitante()); // Retorna a view de criação de requisitante
        }

        // POST: Requisitante/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdReq,Nome,Apelido,Telemovel,Email,Sexo,Pais,Morada,CodigoPostal,Localidade,Nacionalidade,Nif")] Requisitante requisitante, int? processoId)
        {
            if (ModelState.IsValid) // Verifica se o modelo é válido
            {
                requisitante.ProcessoId = processoId.HasValue ? processoId.Value : default; // Associa o ID do processo

                _context.Add(requisitante); // Adiciona o requisitante ao contexto
                await _context.SaveChangesAsync(); // Guarda as alterações

                // Redireciona para os detalhes do processo
                return RedirectToAction("Details", "Processo", new { id = processoId });
            }
            return View(requisitante); // Retorna a view com o requisitante não válido
        }

        // GET: Requisitante/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) // Verifica se o ID é nulo
            {
                return NotFound(); // Retorna erro
            }

            var requisitante = await _context.Requisitante.FindAsync(id); // Busca o requisitante pelo ID
            if (requisitante == null) // Verifica se o requisitante existe
            {
                return NotFound(); // Retorna erro
            }
            return View(requisitante); // Retorna a view de edição do requisitante
        }

        // POST: Requisitante/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReq,Nome,Apelido,Telemovel,Email,Sexo,Pais,Morada,CodigoPostal,Localidade,Nacionalidade,Nif")] Requisitante requisitante, int? processoId)
        {
            if (id != requisitante.IdReq) // Verifica se o ID da receita corresponde
            {
                return NotFound(); // Retorna erro se não corresponder
            }

            if (ModelState.IsValid) // Verifica se o modelo é válido
            {
                try
                {
                    requisitante.ProcessoId = (int)processoId; // Associa o ID do processo

                    _context.Update(requisitante); // Atualiza o requisitante no contexto
                    await _context.SaveChangesAsync(); // Salva as alterações
                }
                catch (DbUpdateConcurrencyException) // Captura exceção de concorrência
                {
                    if (!RequisitanteExists(requisitante.IdReq)) // Verifica se o requisitante ainda existe
                    {
                        return NotFound(); // Retorna erro se não existir
                    }
                    else
                    {
                        throw; // Lança exceção se ocorrer outro erro
                    }
                }
                return RedirectToAction("Details", "Processo", new { id = processoId }); // Redireciona para os detalhes do processo
            }
            return View(requisitante); // Retorna a view com o requisitante não válido
        }

        // GET: Requisitante/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // Verifica se o ID é nulo
            {
                return NotFound(); // Retorna erro
            }

            var requisitante = await _context.Requisitante
                .FirstOrDefaultAsync(m => m.IdReq == id); // Busca o requisitante pelo ID
            if (requisitante == null) // Verifica se o requisitante existe
            {
                return NotFound(); // Retorna erro
            }

            return View(requisitante); // Retorna a view de confirmação da eliminação do requisitante
        }

        // POST: Requisitante/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requisitante = await _context.Requisitante.FindAsync(id); // Busca o requisitante pelo ID
            if (requisitante != null) // Verifica se o requisitante existe
            {
                _context.Requisitante.Remove(requisitante); // Remove o requisitante do contexto
            }

            await _context.SaveChangesAsync(); // Guarda as alterações
            return RedirectToAction(nameof(Index)); // Redireciona para a lista de requisitantes
        }

        private bool RequisitanteExists(int id)
        {
            return _context.Requisitante.Any(e => e.IdReq == id); // Verifica se o requisitante existe
        }
    }
}
