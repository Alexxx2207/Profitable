using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Profitable.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Data.Seeding
{
    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<Trader>>();

            await SeedUserAsync(userManager, "as@as.as", "as@as.as", "Alexander", "Ivanov", "123456", role: GlobalConstants.GlobalDatabaseConstants.TraderRoleName);
        }

        private static async Task SeedUserAsync(UserManager<Trader> userManager,
            string email = null, string userName = null, string firstName = null, string lastName = null, string password = null,
            string profilePicture = null, string role = null)
        {
            var userExists = await userManager.FindByNameAsync(userName);
            if (userExists == null)
            {
                var user = new Trader()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    ProfilePicture = profilePicture,
                    UserName = userName,
                };

                var result = await userManager.CreateAsync(user, password);
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
