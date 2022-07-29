using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Profitable.Data.Seeding.Seeders.Contracts;
using Profitable.Models.EntityModels;
using Profitable.Models.InputModels.Seedeing;

namespace Profitable.Data.Seeding.Seeders
{
    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var trader in GlobalConstants.GlobalDatabaseConstants.DefaultUsersToSeed)
            {
                await SeedUserAsync(userManager, trader, role: GlobalConstants.GlobalDatabaseConstants.TraderRoleName);
            }

        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, SeededTrader trader, string role = null)
        {
            var userExists = await userManager.FindByNameAsync(trader.Email);
            if (userExists == null)
            {
                var user = new ApplicationUser()
                {
                    Id = Guid.NewGuid(),
                    Email = trader.Email,
                    UserName = trader.Email,
                    FirstName = trader.FirstName,
                    LastName = trader.LastName,
                    ProfilePictureURL = trader.ProfilePictureURL,
                };

                var result = await userManager.CreateAsync(user, trader.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
                else
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
