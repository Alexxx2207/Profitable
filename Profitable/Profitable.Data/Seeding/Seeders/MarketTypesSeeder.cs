using Profitable.Data.Repository;
using Profitable.Data.Seeding.Seeders.Contracts;
using Profitable.Models.EntityModels;
using System.Text.Json;

namespace Profitable.Data.Seeding.Seeders
{
    public class MarketTypesSeeder : ISeeder
    {
        public async Task SeedAsync(
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider = null)
        {
            var marketTypeRepository = new Repository<MarketType>(dbContext);

            IAsyncEnumerable<string> typesInput = null;

            using (var stream = new FileStream(
                "DataToSeed/MarketTypes.json",
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
                        var marketType = new MarketType();
                        marketType.Name = type;

                        await marketTypeRepository.AddAsync(marketType);
                    }
                }
            }
        }
    }
}
