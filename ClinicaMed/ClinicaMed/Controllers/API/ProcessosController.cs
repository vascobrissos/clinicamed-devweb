using Microsoft.AspNetCore.Mvc;
using ClinicaMed.Models;
using System;
using System.Linq;
using ClinicaMed.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ClinicaMed.Controllers
{
    // Define a rota base para este controlador de API
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProcessosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método GET para obter todos os processos
        // Rota: GET: api/Processo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Processo>>> GetProcessos()
        {
            // Consulta a base de dados para obter todos os processos, incluindo os examinados
            var processos = await _context.Processo
                .Include(p => p.Examinandos)
                .ToListAsync();

            // Retorna a lista de processos com um código de resposta HTTP 200 (OK)
            return Ok(processos);
        }

        // Método GET para obter um processo específico por ID
        // Rota: GET: api/Processo/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Processo>> GetProcesso(int id)
        {
            // Consulta a base de dados para obter um processo específico pelo ID, incluindo dados relacionados
            var processo = await _context.Processo
                .Include(p => p.Examinandos)
                .Include(p => p.Requisitantes)
                .Include(p => p.ListaProceColab)
                    .ThenInclude(pc => pc.Colaborador)
                .Include(p => p.ListaReceita)
                .Include(p => p.ListaConsulta)
                .FirstOrDefaultAsync(p => p.IdPro == id);

            // Se o processo não for encontrado, retorna um código de resposta HTTP 404 (Not Found)
            if (processo == null)
            {
                return NotFound();
            }

            // Retorna o processo encontrado com um código de resposta HTTP 200 (OK)
            return Ok(processo);
        }
    }
}
