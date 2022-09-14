using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Profitable.Common.Models;
using Profitable.Data.Repository;
using Profitable.Data.Seeding.Seeders.Contracts;
using Profitable.Models.EntityModels;
using System.Text.Json;

namespace Profitable.Data.Seeding.Seeders
{
    public class FuturesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
			var futuresContractRepository = new Repository<FuturesContract>(dbContext);

			IAsyncEnumerable<JsonFutures> typesInput = null;

			using (var stream = new FileStream("DataToSeed/FuturesInformation.json", FileMode.Open, FileAccess.Read))
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

						await futuresContractRepository.AddAsync(futuresContract);
					}
				}
			}
		}
		private class JsonFutures
		{
			public string Name { get; set; }

			public string TickSize { get; set; }

			public string TickValue { get; set; }
		}
	}
}
