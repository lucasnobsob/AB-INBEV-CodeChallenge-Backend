using AB_INBEV.Infra.CrossCutting.Identity.Data;
using AB_INBEV.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AB_INBEV.Services.API.StartupExtensions
{
    public static class MigrationExtension
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context1 = services.GetRequiredService<ApplicationDbContext>();
                var context2 = services.GetRequiredService<EventStoreSqlContext>();
                var context3 = services.GetRequiredService<AuthDbContext>();

                context1.Database.Migrate();
                context2.Database.Migrate();
                context3.Database.Migrate();

                SeedRoles(services).Wait();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Error applying migrations.");
            }
        }

        private static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string roleName = "Admin";

            bool roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

    }
}
