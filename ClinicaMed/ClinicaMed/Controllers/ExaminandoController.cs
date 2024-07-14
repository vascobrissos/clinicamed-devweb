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
            ViewData["ProcessoId"] = processoId; // Passar o ID do processo para a View
            return View(await _context.Examinando.ToListAsync()); // Retornar a lista de examinandos para a view
        }

        // GET: Examinando/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // Verifica se o ID é nulo
            {
                return NotFound(); // Retorna erro
            }

            // Procurar o examinando pelo ID
            var examinando = await _context.Examinando
                .FirstOrDefaultAsync(m => m.IdExa == id);
            if (examinando == null) // Verifica se o examinando existe
            {
                return NotFound(); // Retorna erro
            }

            return View(examinando); // Retorna a view com os detalhes do examinando
        }

        // GET: Examinando/Create
        public IActionResult Create(int? processoId)
        {
            ViewBag.ProcessoId = processoId; // Passar o ID do processo para a View
            return View(new Examinando()); // Retornar a view para criar um novo examinando
        }

        // POST: Examinando/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdExa,Nome,Apelido,Telemovel,Email,Sexo,Antecedentes,DataNascimento,Profissao,Pais,Morada,CodigoPostal,Localidade,Nacionalidade,NumUtente,CartaoCidadao,ValidadeCC,Nif,Seguradora,NumeroSeguro,FiliacaoMae,FiliacaoPai")] Examinando examinando, int? processoId)
        {
            if (ModelState.IsValid) // Verifica se o estado do modelo é válido
            {
                examinando.ProcessoId = (int)processoId; // Associar o ID do processo ao examinando

                _context.Add(examinando); // Adiciona o examinando ao contexto
                await _context.SaveChangesAsync(); // Salva as alterações na base de dados

                // Redireciona para os detalhes do processo
                return RedirectToAction("Details", "Processo", new { id = processoId });
            }
            return View(examinando); // Retorna a view com o modelo do examinando se não for válido
        }

        // GET: Examinando/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) // Verifica se o ID é nulo
            {
                return NotFound(); // Retorna erro
            }

            var examinando = await _context.Examinando.FindAsync(id); // Procura o examinando pelo ID
            if (examinando == null) // Verifica se o examinando existe
            {
                return NotFound(); // Retorna erro
            }
            return View(examinando); // Retorna a view de edição com o examinando
        }

        // POST: Examinando/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdExa,Nome,Apelido,Telemovel,Email,Sexo,Antecedentes,DataNascimento,Profissao,Pais,Morada,CodigoPostal,Localidade,Nacionalidade,NumUtente,CartaoCidadao,ValidadeCC,Nif,Seguradora,NumeroSeguro,FiliacaoMae,FiliacaoPai")] Examinando examinando, int? processoId)
        {
            if (id != examinando.IdExa) // Verifica se o ID da consulta corresponde ao ID fornecido
            {
                return NotFound(); // Retorna erro se não corresponder
            }

            if (ModelState.IsValid) // Verifica se o estado do modelo é válido
            {
                try
                {
                    examinando.ProcessoId = (int)processoId; // Associar o ID do processo ao examinando

                    _context.Update(examinando); // Atualiza o examinando no contexto
                    await _context.SaveChangesAsync(); // Salva as alterações
                }
                catch (DbUpdateConcurrencyException) // Captura exceções de concorrência
                {
                    if (!ExaminandoExists(examinando.IdExa)) // Verifica se o examinando ainda existe
                    {
                        return NotFound(); // Retorna erro se não existir
                    }
                    else
                    {
                        throw; // Re-lança a exceção
                    }
                }
                return RedirectToAction("Details", "Processo", new { id = processoId }); // Redireciona para os detalhes do processo
            }
            return View(examinando); // Retorna a view com o modelo do examinando se o estado não for válido
        }

        // GET: Examinando/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // Verifica se o ID é nulo
            {
                return NotFound(); // Retorna erro
            }

            var examinando = await _context.Examinando
                .FirstOrDefaultAsync(m => m.IdExa == id); // Procura o examinando pelo ID
            if (examinando == null) // Verifica se o examinando existe
            {
                return NotFound(); // Retorna erro
            }

            return View(examinando); // Retorna a view de confirmação de eliminação com o examinando
        }

        // POST: Examinando/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var examinando = await _context.Examinando.FindAsync(id); // Encontrar o examinando pelo ID
            if (examinando != null) // Verifica se o examinando existe
            {
                _context.Examinando.Remove(examinando); // Remove o examinando do contexto
            }

            await _context.SaveChangesAsync(); // Salva as alterações
            return RedirectToAction(nameof(Index)); // Redireciona para a lista de examinandos
        }

        private bool ExaminandoExists(int id)
        {
            return _context.Examinando.Any(e => e.IdExa == id); // Verifica se o examinando existe na base de dados
        }
    }
}
