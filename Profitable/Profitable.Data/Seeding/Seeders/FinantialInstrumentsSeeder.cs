namespace Profitable.Data.Seeding.Seeders
{
	using Profitable.Data.Repository;
	using Profitable.Data.Seeding.Seeders.Contracts;
	using Profitable.Models.EntityModels;
	using System.Text.Json;
	public class FinantialInstrumentsSeeder : ISeeder
	{
		public async Task SeedAsync(
			ApplicationDbContext dbContext,
			IServiceProvider serviceProvider = null)
		{
			var instrumentRepository = new Repository<FinancialInstrument>(dbContext);

			using (var stream = new FileStream(
				"DataToSeed/TicketSymbols.json",
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
						var exchange = dbContext.Exchanges.FirstOrDefault(e => e.Name == instrument.Exchange);
						var marketType = dbContext.MarketTypes.FirstOrDefault(e => e.Name == instrument.Type);

						var finInstrument = new FinancialInstrument();
						finInstrument.TickerSymbol = instrument.Symbol.ToUpper();
						finInstrument.Exchange = exchange;
						finInstrument.MarketType = marketType;

						await instrumentRepository.AddAsync(finInstrument);
					}
				}
			}
		}

		private class JsonInstrument
		{
			public string Symbol { get; set; }

			public string Type { get; set; }

			public string Exchange { get; set; }
		}
	}
}
