using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicaMed.Models;
using ClinicaMed.Data;
using Microsoft.AspNetCore.Identity;

namespace ClinicaMed.Controllers
{
    public class ColaboradorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ColaboradorController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Colaboradores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Colaborador.ToListAsync());
        }

        // GET: Colaboradores/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Colaboradores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string EmailCol, string Role, [Bind("UserId,NomeApresentacao,Nome,Apelido,Telemovel,Sexo,DataNascimento,Pais,Morada,CodPostal,Localidade,Nacionalidade,Nif,EstadoCivil,NumOrdem")] Colaborador colaborador)
        {

            if (ModelState.IsValid)
            {
                // Criação do utilizador na ASP.NET Core Identity com password específica
                var apelidoParts = colaborador.Apelido.Split(' ');
                string FormattedUsername = colaborador.Nome[0].ToString().ToLower() + apelidoParts[0].ToLower();

                var user = new IdentityUser { UserName = FormattedUsername, Email = EmailCol, EmailConfirmed=true};
                var result = await _userManager.CreateAsync(user, "Alterame123@@");

                if (result.Succeeded)
                {
                    // Adicionar o colaborador ao contexto da BD
                    _context.Colaborador.Add(colaborador);

                    // Associar o ID do utilizador ao colaborador criado
                    colaborador.UserId = user.Id;

                    await _userManager.AddToRoleAsync(user, Role);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    // Se falhou ao criar o utilizador
                }
            }

            // Se chegou aqui, significa que houve um erro no ModelState, então retorna para a view com o viewModel
            return View(colaborador);
        }

        // GET: Colaboradores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colaborador = await _context.Colaborador.FindAsync(id);
            if (colaborador == null)
            {
                return NotFound();
            }

            // Obter o utilizador da ASP.NET Core Identity associado ao colaborador
            var user = await _userManager.FindByIdAsync(colaborador.UserId);
            if (user == null)
            {
                return NotFound();
            }

            // Adicionar dados extras ao ViewData ou ViewBag

            // Carregar email e roles do utilizador para exibição na View de edição
            ViewData["UserEmail"] = user.Email; // Adicione uma propriedade EmailCol ao modelo Colaborador se necessário
            ViewData["UserRole"] = (await _userManager.GetRolesAsync(user)).FirstOrDefault(); // Adicione uma propriedade Role ao modelo Colaborador se necessário

            return View(colaborador);
        }

        // POST: Colaboradores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string EmailCol, string Role, [Bind("IdCol, UserId,NomeApresentacao,Nome,Apelido,Telemovel,Sexo,DataNascimento,Pais,Morada,CodPostal,Localidade,Nacionalidade,Nif,EstadoCivil,NumOrdem")] Colaborador colaborador)
        {
            if (id != colaborador.IdCol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Atualizar o colaborador no contexto do banco de dados
                    _context.Update(colaborador);
                    await _context.SaveChangesAsync();

                    // Verificar se há alteração no email para atualizar na ASP.NET Core Identity
                    var user = await _userManager.FindByIdAsync(colaborador.UserId);
                    if (user != null && user.Email != EmailCol)
                    {
                        var setEmailResult = await _userManager.SetEmailAsync(user, EmailCol);
                        if (!setEmailResult.Succeeded)
                        {
                            foreach (var error in setEmailResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            return View(colaborador);
                        }
                    }

                    // Verificar se há alteração na role para atualizar na ASP.NET Core Identity
                    var roles = await _userManager.GetRolesAsync(user);
                    if (!roles.Contains(Role))
                    {
                        var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, roles);
                        var addRoleResult = await _userManager.AddToRoleAsync(user, Role);
                        if (!addRoleResult.Succeeded)
                        {
                            foreach (var error in addRoleResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            return View(colaborador);
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColaboradorExists(colaborador.IdCol))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(colaborador);
        }



        //ALTERAR ABAIXO PARA DESATIVAR/ATIVAR EM VEZ DE APAGAR (users aumentar lockoutend)


        // GET: Colaboradores/Toggle/5
        public async Task<IActionResult> Toggle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colaborador = await _context.Colaborador
                .FirstOrDefaultAsync(m => m.IdCol == id);
            if (colaborador == null)
            {
                return NotFound();
            }

            return View(colaborador);
        }

        // POST: Colaboradores/Toggle/5
        [HttpPost, ActionName("Toggle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleConfirmed(int id)
        {
            var colaborador = await _context.Colaborador.FindAsync(id);
            if (colaborador != null)
            {
                _context.Colaborador.Remove(colaborador);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ColaboradorExists(int id)
        {
            return _context.Colaborador.Any(e => e.IdCol == id);
        }
    }
}
