using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Profitable.Common.GlobalConstants;
using Profitable.Data.Seeding.Seeders.Contracts;

namespace Profitable.Data.Seeding.Seeders
{
    public class RoleSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            await SeedRoleAsync(roleManager, GlobalDatabaseConstants.TraderRoleName);
        }

        private static async Task SeedRoleAsync(RoleManager<IdentityRole<Guid>> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
