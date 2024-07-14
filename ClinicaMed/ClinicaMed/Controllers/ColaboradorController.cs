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
using ClinicaMed.ViewModels;
using ClinicaMed.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ClinicaMed.Controllers
{
    [Authorize(Roles = "Administrador, Administrativo")]
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
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString; // Armazenar o filtro atual para a view

            // Obter todos os colaboradores da base de dados
            var colaboradores = await _context.Colaborador.ToListAsync();
            var viewModel = new List<ColaboradorViewModel>(); // Lista para armazenar os colaboradores formatados para a view

            foreach (var colaborador in colaboradores)
            {
                // Encontrar o utilizador associado ao colaborador
                var user = await _userManager.FindByIdAsync(colaborador.UserId);
                var roles = await _userManager.GetRolesAsync(user); // Obter roles do utilizador
                var role = roles.FirstOrDefault(); // Pegar na primeira role, se existir
                var username = await _userManager.GetUserNameAsync(user); // Obter o nome de utilizador
                var email = await _userManager.GetEmailAsync(user); // Obter o email
                var accountStatus = (user.LockoutEnd == null || user.LockoutEnd <= DateTime.Now) ? "Ativa" : "Inativa"; // Verificar o estado da conta

                // Adicionar ao view model
                viewModel.Add(new ColaboradorViewModel
                {
                    Colaborador = colaborador,
                    Role = role,
                    Username = username,
                    Email = email,
                    AccountStatus = accountStatus
                });
            }

            // Se o utilizador fez uma pesquisa, filtrar os colaboradores pelo nome de utilizador
            if (!String.IsNullOrEmpty(searchString))
            {
                viewModel = viewModel.Where(v => v.Username.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(viewModel); // Retornar a view com o modelo filtrado
        }

        // GET: Colaboradores/Create
        public IActionResult Create()
        {
            return View(); // Retornar a view para criar um novo colaborador
        }

        // POST: Colaboradores/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF
        public async Task<IActionResult> Create(string EmailCol, string Role, [Bind("UserId,NomeApresentacao,Nome,Apelido,Telemovel,Sexo,DataNascimento,Pais,Morada,CodPostal,Localidade,Nacionalidade,Nif,EstadoCivil,NumOrdem")] Colaborador colaborador)
        {
            if (ModelState.IsValid) // Verifica se o estado do modelo é válido
            {
                // Criação do utilizador na ASP.NET Core Identity com password específica
                var apelidoParts = colaborador.Apelido.Split(' '); // Separar o apelido em partes
                string FormattedUsername = colaborador.Nome[0].ToString().ToLower() + apelidoParts[0].ToLower(); // Gerar um nome de utilizador formatado

                var user = new IdentityUser { UserName = FormattedUsername, Email = EmailCol, EmailConfirmed = true }; // Criar um novo utilizador
                var result = await _userManager.CreateAsync(user, "Alterame123@@"); // Criar o utilizador com uma password padrão

                if (result.Succeeded) // Verificar se a criação foi bem-sucedida
                {
                    _context.Colaborador.Add(colaborador); // Adicionar o colaborador ao contexto da base de dados
                    colaborador.UserId = user.Id; // Associar o ID do utilizador ao colaborador criado

                    await _userManager.AddToRoleAsync(user, Role); // Adicionar a role ao utilizador

                    await _context.SaveChangesAsync(); // Guardar as alterações na base de dados

                    return RedirectToAction(nameof(Index)); // Redirecionar para a lista de colaboradores
                }
                else
                {
                    // Se falhou ao criar o utilizador, adicionar os erros ao ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // Se chegou aqui, significa que houve um erro no ModelState, então retorna para a view com o modelo
            return View(colaborador);
        }

        // GET: Colaboradores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) // Verifica se o ID é nulo
            {
                return NotFound(); // Retorna erro se o ID não for fornecido
            }

            var colaborador = await _context.Colaborador.FindAsync(id); // Procura o colaborador pelo ID
            if (colaborador == null) // Verifica se o colaborador existe
            {
                return NotFound(); // Retorna erro se não existir
            }

            // Obter o utilizador da ASP.NET Core Identity associado ao colaborador
            var user = await _userManager.FindByIdAsync(colaborador.UserId);
            if (user == null)
            {
                return NotFound(); // Retorna erro se o utilizador não existir
            }

            // Adicionar dados adicionais ao ViewData para serem utilizados na view
            ViewData["UserEmail"] = user.Email; // Email do utilizador
            ViewData["UserRole"] = (await _userManager.GetRolesAsync(user)).FirstOrDefault(); // Primeira role do utilizador

            return View(colaborador); // Retornar a view de edição com o colaborador
        }

        // POST: Colaboradores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string EmailCol, string Role, [Bind("IdCol, UserId,NomeApresentacao,Nome,Apelido,Telemovel,Sexo,DataNascimento,Pais,Morada,CodPostal,Localidade,Nacionalidade,Nif,EstadoCivil,NumOrdem")] Colaborador colaborador)
        {
            if (id != colaborador.IdCol) // Verifica se o ID do colaborador corresponde ao ID da URL
            {
                return NotFound(); // Retorna erro se não corresponder
            }

            if (ModelState.IsValid) // Verifica se o estado do modelo é válido
            {
                try
                {
                    // Atualizar o colaborador no contexto da base de dados
                    _context.Update(colaborador);
                    await _context.SaveChangesAsync(); // Salvar alterações

                    // Verificar se há alteração no email para atualizar na ASP.NET Core Identity
                    var user = await _userManager.FindByIdAsync(colaborador.UserId);
                    if (user != null && user.Email != EmailCol) // Verifica se o email foi alterado
                    {
                        var setEmailResult = await _userManager.SetEmailAsync(user, EmailCol); // Atualiza o email
                        if (!setEmailResult.Succeeded) // Verifica se a atualização foi bem-sucedida
                        {
                            foreach (var error in setEmailResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description); // Adiciona erros ao ModelState
                            }
                            return View(colaborador); // Retorna a view com os erros
                        }
                    }

                    // Verificar se há alteração na role
                    var roles = await _userManager.GetRolesAsync(user);
                    if (!roles.Contains(Role)) // Se a nova role não estiver nas roles atuais
                    {
                        var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, roles); // Remove as roles existentes
                        var addRoleResult = await _userManager.AddToRoleAsync(user, Role); // Adiciona a nova role
                        if (!addRoleResult.Succeeded) // Verifica se a adição foi bem-sucedida
                        {
                            foreach (var error in addRoleResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description); // Adiciona erros ao ModelState
                            }
                            return View(colaborador); // Retorna a view com os erros
                        }
                    }

                    return RedirectToAction(nameof(Index)); // Redireciona para a lista de colaboradores
                }
                catch (DbUpdateConcurrencyException) // Captura exceções de concorrência
                {
                    if (!ColaboradorExists(colaborador.IdCol)) // Verifica se o colaborador existe
                    {
                        return NotFound(); // Retorna erro se não existir
                    }
                    else
                    {
                        throw; // Relança a exceção se não for uma questão de não existir
                    }
                }
            }
            return View(colaborador); // Retorna a view com o colaborador se o ModelState não for válido
        }

        // GET: Colaboradores/Toggle/5
        public async Task<IActionResult> Toggle(int? id)
        {
            if (id == null) // Verifica se o ID é nulo
            {
                return NotFound(); // Retorna erro 404
            }

            // Obtém o colaborador com o ID fornecido
            var colaborador = await _context.Colaborador
                .FirstOrDefaultAsync(m => m.IdCol == id);
            if (colaborador == null)
            {
                return NotFound(); // Retorna erro se não existir
            }

            // Obter o utilizador associado ao colaborador
            var user = await _userManager.FindByIdAsync(colaborador.UserId);
            if (user == null)
            {
                return NotFound(); // Retorna erro se não existir
            }

            // Verifica o estado de LockoutEnd e atualiza a data
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow)
            {
                // Desativar a conta (configurar LockoutEnd para null)
                await _userManager.SetLockoutEndDateAsync(user, null);
            }
            else
            {
                // Ativar a conta (definir LockoutEnd para uma data futura)
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100)); // ou outro valor
            }

            await _context.SaveChangesAsync(); // Guardar alterações
            return RedirectToAction(nameof(Index)); // Redirecionar para a lista de colaboradores
        }

        // POST: Colaboradores/Toggle/5
        [HttpPost, ActionName("Toggle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleConfirmed(int id)
        {
            var colaborador = await _context.Colaborador.FindAsync(id); // Encontrar o colaborador pelo ID
            if (colaborador != null)
            {
                var user = await _userManager.FindByIdAsync(colaborador.UserId); // Encontrar o utilizador associado
                if (user != null)
                {
                    // Alternar o estado de LockoutEnd
                    if (user.LockoutEnd == null || user.LockoutEnd <= DateTime.Now)
                    {
                        // Desativar a conta (definir LockoutEnd para uma data futura)
                        user.LockoutEnd = DateTime.Now.AddYears(100); // Configura LockoutEnd para 100 anos no futuro
                    }
                    else
                    {
                        // Ativar a conta (definir LockoutEnd para null)
                        user.LockoutEnd = null; // Remove o lockout
                    }

                    await _userManager.UpdateAsync(user); // Atualiza o utilizador
                }
                else
                {
                    return NotFound(); // Retorna erro se o utilizador não existir
                }
            }
            else
            {
                return NotFound(); // Retorna erro se o colaborador não existir
            }
            return RedirectToAction(nameof(Index)); // Redirecionar para a lista de colaboradores
        }

        // Método privado para verificar se um colaborador existe
        private bool ColaboradorExists(int id)
        {
            return _context.Colaborador.Any(e => e.IdCol == id); // Retorna verdadeiro se o colaborador existir na base de dados
        }
    }
}
