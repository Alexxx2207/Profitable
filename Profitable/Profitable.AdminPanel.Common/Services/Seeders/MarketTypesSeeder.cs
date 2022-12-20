namespace Profitable.AdminPanel.Common.Services.Seeders
{
	using Profitable.AdminPanel.Common.Services.Seeders.Contracts;
	using Profitable.Data;
	using Profitable.Models.EntityModels;
	using System.Text.Json;

	public class MarketTypesSeeder : ISeeder
	{
		public async Task SeedAsync(ApplicationDbContext dbContext)
		{
			IAsyncEnumerable<string> typesInput = null;

			using (var stream = new FileStream(
				"Services/Seeders/DataToSeed/MarketTypes.json",
				FileMode.Open,
				FileAccess.Read))
			{
				typesInput = JsonSerializer.DeserializeAsyncEnumerable<string>
					(stream, new JsonSerializerOptions()
					{
						AllowTrailingCommas = true,
						PropertyNameCaseInsensitive = true,
					});

				var currentEntries = dbContext.MarketTypes;

				await foreach (var type in typesInput.Distinct())
				{
					if (!currentEntries.Any(e => e.Name == type))
					{
						var marketType = new MarketType
						{
							Name = type
						};

						await dbContext.AddAsync(marketType);
					}
				}
			}
			await dbContext.SaveChangesAsync();
		}
	}
}
