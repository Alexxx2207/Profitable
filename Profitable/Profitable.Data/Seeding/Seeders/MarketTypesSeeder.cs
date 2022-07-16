using Newtonsoft.Json;
using Profitable.Data.Repository;
using Profitable.Data.Seeding.Seeders.Contracts;
using Profitable.Models.EntityModels;


namespace Profitable.Data.Seeding.Seeders
{
    public class MarketTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider = null)
        {
            var marketTypeRepository = new Repository<MarketType>(dbContext);

            var types = JsonConvert.DeserializeObject<List<string>>(await new StreamReader("DataToSeed/MarketTypes.json").ReadToEndAsync());

            var currentEntries = dbContext.MarketTypes;

            foreach (var instrument in types.DistinctBy(type => type))
            {
                if (!currentEntries.Any(e => e.Name == instrument))
                {
                    var marketType = new MarketType();
                    marketType.Name = instrument;

                    await marketTypeRepository.AddAsync(marketType);
                }
            }

            await marketTypeRepository.SaveChangesAsync();
        }
    }
}
