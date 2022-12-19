namespace Profitable.AdminPanel.Common.Services.Seeders.Contracts
{
	using Profitable.Data;

	public interface ISeeder
	{
		Task SeedAsync(ApplicationDbContext dbContext);
	}
}
