using ClinicaMed.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaMed.Data
{
    public static class DBInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

            await dbContext.Database.MigrateAsync();

            // Adicionar admin
            if (!await userManager.Users.AnyAsync())
            {
                var user = new IdentityUser
                {
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@admin.com",
                    NormalizedEmail = "ADMIN@ADMIN.COM",
                    EmailConfirmed = true,
                    SecurityStamp = "5ZPZEF6SBW7IU4M344XNLT4NN5RO4GRU",
                    ConcurrencyStamp = "c86d8254-dd50-44be-8561-d2d44d4bbb2f"
                };

                var result = await userManager.CreateAsync(user, "Admin123#");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrador");

                    // Criar um colaborador associado ao utilizador admin
                    var colaborador = new Colaborador
                    {
                        NomeApresentacao = "Admin Clinica",
                        Nome = "Admin",
                        Apelido = "OG",
                        Telemovel = "999999999",
                        Sexo = 0,
                        DataNascimento = DateOnly.Parse("1969-04-10"),
                        Pais = "Portugal",
                        Morada = "Rua do Admin",
                        CodPostal = "9999-999",
                        Localidade = "Admin Turf",
                        Nacionalidade = "Tuga",
                        Nif = "999999999",
                        EstadoCivil = 0,
                        NumOrdem = 99999,
                        UserId = user.Id
                    };

                    await dbContext.Colaborador.AddAsync(colaborador);
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
