namespace Profitable.AdminPanel.Common.Services.Seeders
{
	using Profitable.AdminPanel.Common.Services.Seeders.Contracts;
	using Profitable.Data;
	using Profitable.Models.EntityModels;
	using System.Text.Json;

	public class ExchangesSeeder : ISeeder
	{
		public async Task SeedAsync(ApplicationDbContext dbContext)
		{
			IAsyncEnumerable<string> exchangesInput = null;

			using (var stream = new FileStream(
                "Services/Seeders/DataToSeed/Exchanges.json",
				FileMode.Open,
				FileAccess.Read))
			{
				exchangesInput = JsonSerializer.DeserializeAsyncEnumerable<string>
					(stream, new JsonSerializerOptions()
					{
						AllowTrailingCommas = true,
						PropertyNameCaseInsensitive = true,
					});

				var currentEntries = dbContext.Exchanges;

				await foreach (var exchange in exchangesInput.Distinct())
				{
					if (!currentEntries.Any(e => e.Name == exchange))
					{
						var exchangeEntity = new Exchange();
						exchangeEntity.Name = exchange;

						await dbContext.AddAsync(exchangeEntity);
					}
				}
			}
			await dbContext.SaveChangesAsync();
		}
	}
}
