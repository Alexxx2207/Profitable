﻿namespace Profitable.AdminPanel.Common.Services.Seeders
{
	using Profitable.AdminPanel.Common.Services.Seeders.Contracts;
	using Profitable.Data;
	using Profitable.Models.EntityModels;
	using System.Text.Json;

	public class FuturesSeeder : ISeeder
	{
		public async Task SeedAsync(ApplicationDbContext dbContext)
		{
			IAsyncEnumerable<JsonFutures> typesInput = null;

			using (var stream = new FileStream(
                "Services/Seeders/DataToSeed/FuturesInformation.json",
				FileMode.Open,
				FileAccess.Read))
			{
				typesInput = JsonSerializer.DeserializeAsyncEnumerable<JsonFutures>
					(stream, new JsonSerializerOptions()
					{
						AllowTrailingCommas = true,
						PropertyNameCaseInsensitive = true,
					});

				var currentEntries = dbContext.FuturesContracts;

				await foreach (var jsonFuturesContract in typesInput.Distinct())
				{
					if (!currentEntries.Any(e => e.Name == jsonFuturesContract.Name))
					{
						var futuresContract = new FuturesContract();
						futuresContract.Name = jsonFuturesContract.Name;
						futuresContract.TickSize = double.Parse(jsonFuturesContract.TickSize);
						futuresContract.TickValue = double.Parse(jsonFuturesContract.TickValue);

						await dbContext.AddAsync(futuresContract);
					}
				}
			}
			await dbContext.SaveChangesAsync();
		}

		private class JsonFutures
		{
			public string Name { get; set; }

			public string TickSize { get; set; }

			public string TickValue { get; set; }
		}
	}
}
