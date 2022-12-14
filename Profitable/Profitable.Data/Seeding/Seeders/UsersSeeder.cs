namespace Profitable.Data.Seeding.Seeders
{
	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.DependencyInjection;
	using Profitable.Common.GlobalConstants;
	using Profitable.Common.Models;
	using Profitable.Data.Seeding.Seeders.Contracts;
	using Profitable.Models.EntityModels;

	public class UsersSeeder : ISeeder
	{
		public async Task SeedAsync(
			ApplicationDbContext dbContext,
			IServiceProvider serviceProvider)
		{
			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			foreach (var trader in GlobalDatabaseConstants.DefaultUsersToSeed)
			{
				await SeedUserAsync(
					userManager,
					trader);
			}

		}

		private static async Task SeedUserAsync(
			UserManager<ApplicationUser> userManager,
			SeededTrader trader)
		{
			var userExists = await userManager.FindByEmailAsync(trader.Email);

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
					throw new Exception(
						string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
				}
			}
		}
	}
}
