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
            /* Esta instrução importa tudo o que está pre-definido
             * na super classe
             */
            base.OnModelCreating(builder);

            /* Adição de dados à Base de Dados
             * Esta forma é PERSISTENTE, pelo que apenas deve ser utilizada em 
             * dados que perduram da fase de 'desenvolvimento' para a fase de 'produção'.
             * Implica efetuar um 'Add-Migration'
             * 
             * Atribuir valores às ROLES
             */
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "adm", Name = "Administrador", NormalizedName = "ADMINISTRADOR" },
                new IdentityRole { Id = "col", Name = "Administrativo", NormalizedName = "ADMINISTRATIVO" },
                new IdentityRole { Id = "med", Name = "Medico", NormalizedName = "MEDICO" }
                );

            builder.Entity<Examinando>()
            .HasOne(e => e.Processo)
            .WithMany(p => p.Examinandos)
            .HasForeignKey(e => e.ProcessoId);

            builder.Entity<Requisitante>()
            .HasOne(e => e.Processo)
            .WithMany(p => p.Requisitantes)
            .HasForeignKey(e => e.ProcessoId);

            builder.Entity<Receita>()
            .HasOne(r => r.Colaborador)
            .WithMany()
            .HasForeignKey(r => r.ColaboradorFK)
            .OnDelete(DeleteBehavior.Restrict);
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
