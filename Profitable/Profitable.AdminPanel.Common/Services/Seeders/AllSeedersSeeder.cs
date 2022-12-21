using Profitable.AdminPanel.Common.Constants;
using Profitable.AdminPanel.Common.Services.Seeders.Contracts;
using Profitable.Data;

namespace Profitable.AdminPanel.Common.Services.Seeders
{
	public class AllSeedersSeeder : ISeeder
	{
		public async Task SeedAsync(ApplicationDbContext dbContext)
		{
			foreach (var seeder in GlobalConstants.SeedChoices)
			{
				await seeder.Seeder.SeedAsync(dbContext);
			}
		}
	}
}
