using ClinicaMed.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ClinicaMed.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Chama o método base para garantir que as configurações predefinidas sejam aplicadas
            base.OnModelCreating(builder);

            // Adiciona dados à base de dados
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "adm", Name = "Administrador", NormalizedName = "ADMINISTRADOR" },
                new IdentityRole { Id = "col", Name = "Administrativo", NormalizedName = "ADMINISTRATIVO" },
                new IdentityRole { Id = "med", Name = "Medico", NormalizedName = "MEDICO" }
            );

            // Configurações das relações entre entidades
            builder.Entity<Examinando>()
                .HasOne(e => e.Processo) // Cada Examinando está associado a um Processo
                .WithMany(p => p.Examinandos) // Um Processo pode ter muitos Examinandos
                .HasForeignKey(e => e.ProcessoId); // Chave estrangeira

            builder.Entity<Requisitante>()
                .HasOne(e => e.Processo) // Cada Requisitante está associado a um Processo
                .WithMany(p => p.Requisitantes) // Um Processo pode ter muitos Requisitantes
                .HasForeignKey(e => e.ProcessoId); // Chave estrangeira

            builder.Entity<Receita>()
                .HasOne(r => r.Colaborador) // Cada Receita está associada a um Colaborador
                .WithMany() // Um Colaborador pode ter muitas Receitas
                .HasForeignKey(r => r.ColaboradorFK) // Chave estrangeira
                .OnDelete(DeleteBehavior.Restrict); // Comportamento em caso de eliminação
        }

        // definição das 'tabelas'

        /// <summary>
        /// Tabela Colaboradores
        /// </summary>
        public DbSet<Colaborador> Colaborador { get; set; }

        /// <summary>
        /// tabela Consultas
        /// </summary>
        public DbSet<Consulta> Consulta { get; set; }

        /// <summary>
        /// tabela Examinandos
        /// </summary>
        public DbSet<Examinando> Examinando { get; set; }

        /// <summary>
        /// Tabela Processos
        /// </summary>
        public DbSet<Processo> Processo { get; set; }

        /// <summary>
        /// tabela ProcessoColaborador
        /// </summary>
        public DbSet<ProcessoColaborador> ProcessoColaborador { get; set; }

        /// <summary>
        /// tabela Receitas
        /// </summary>
        public DbSet<Receita> Receita { get; set; }

        /// <summary>
        /// tabela Requisitantes
        /// </summary>
        public DbSet<Requisitante> Requisitante { get; set; }
    }
}
