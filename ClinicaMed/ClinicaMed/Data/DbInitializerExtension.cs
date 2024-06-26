using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ClinicaMed.Data
{
    public static class DbInitializerExtension
    {
        public static async Task<IApplicationBuilder> UseItToSeedSqlServer(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                await DBInitializer.InitializeAsync(context, userManager);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            }

            return app;
        }
    }
}
