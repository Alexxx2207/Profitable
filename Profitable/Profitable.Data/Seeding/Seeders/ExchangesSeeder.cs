using Profitable.Data.Repository;
using Profitable.Data.Seeding.Seeders.Contracts;
using Profitable.Models.EntityModels;
using System.Text.Json;

namespace Profitable.Data.Seeding.Seeders
{
    public class ExchangesSeeder : ISeeder
    {
        public async Task SeedAsync(
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider = null)
        {
            var exchangeRepository = new Repository<Exchange>(dbContext);

            IAsyncEnumerable<string> exchangesInput = null;

            using (var stream = new FileStream(
                "DataToSeed/Exchanges.json",
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

                        await exchangeRepository.AddAsync(exchangeEntity);
                    }
                }
            }
        }
    }
}
