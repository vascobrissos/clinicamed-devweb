using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicaMed.Data;
using ClinicaMed.Models;
using ClinicaMed.ViewModels;
using Microsoft.AspNetCore.Identity;

[Route("api/[controller]")]
[ApiController]
public class ColaboradoresController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ColaboradoresController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Método HTTP GET para obter a lista de colaboradores
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ColaboradorViewModel>>> GetColaboradores()
    {
        // Obtém a lista de todos os colaboradores da base de dados
        var colaboradores = await _context.Colaborador.ToListAsync();
        // Cria uma lista de ColaboradorViewModel para armazenar os dados a serem retornados
        var viewModel = new List<ColaboradorViewModel>();

        // Para cada colaborador na lista de colaboradores
        foreach (var colaborador in colaboradores)
        {
            // Obtém o utilizador associado ao colaborador pelo seu UserId
            var user = await _userManager.FindByIdAsync(colaborador.UserId);
            // Obtém os papéis (roles) atribuídos ao utilizador
            var roles = await _userManager.GetRolesAsync(user);
            // Obtém o primeiro papel da lista de papéis
            var role = roles.FirstOrDefault();
            // Obtém o nome de utilizador
            var username = await _userManager.GetUserNameAsync(user);
            // Obtém o email do utilizador
            var email = await _userManager.GetEmailAsync(user);
            // Determina o estado da conta com base na data de fim do lockout
            var accountStatus = (user.LockoutEnd == null || user.LockoutEnd <= DateTime.Now) ? "Ativa" : "Inativa";

            // Adiciona os dados do colaborador e informações do utilizador na lista de ViewModel
            viewModel.Add(new ColaboradorViewModel
            {
                Colaborador = colaborador,
                Role = role,
                Username = username,
                Email = email,
                AccountStatus = accountStatus
            });
        }

        // Retorna a lista de ViewModel com um código HTTP 200 (OK)
        return Ok(viewModel);
    }
}
