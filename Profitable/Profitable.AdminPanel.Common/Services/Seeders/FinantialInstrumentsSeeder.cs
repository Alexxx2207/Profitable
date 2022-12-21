namespace Profitable.AdminPanel.Common.Services.Seeders
{
	using Profitable.AdminPanel.Common.Services.Seeders.Contracts;
	using Profitable.Data;
	using Profitable.Models.EntityModels;
	using System.Text.Json;

	public class FinantialInstrumentsSeeder : ISeeder
	{
		public async Task SeedAsync(ApplicationDbContext dbContext)
		{
			using (var stream = new FileStream(
                "Services/Seeders/DataToSeed/TicketSymbols.json",
				FileMode.Open,
				FileAccess.Read))
			{
				var instrumentsInput = JsonSerializer.DeserializeAsyncEnumerable<JsonInstrument>
					(stream, new JsonSerializerOptions()
					{
						AllowTrailingCommas = true,
						PropertyNameCaseInsensitive = true,
					});

				var currentEntries = dbContext.FinancialInstruments;

				await foreach (var instrument in instrumentsInput)
				{
					if (!currentEntries.Any(e => e.TickerSymbol == instrument.Symbol))
					{
						var exchange = dbContext.Exchanges
							.FirstOrDefault(e => e.Name == instrument.Exchange);

						var marketType = dbContext.MarketTypes
							.FirstOrDefault(e => e.Name == instrument.Type);

						if (exchange != null && marketType != null)
						{
							var finInstrument = new FinancialInstrument();
							finInstrument.TickerSymbol = instrument.Symbol.ToUpper();
							finInstrument.Exchange = exchange;
							finInstrument.MarketType = marketType;

							await dbContext.AddAsync(finInstrument);
						}
					}
				}
			}
			await dbContext.SaveChangesAsync();
		}

		private class JsonInstrument
		{
			public string Symbol { get; set; }

			public string Type { get; set; }

			public string Exchange { get; set; }
		}
	}
}
