using Newtonsoft.Json;
using Profitable.Data.Repository;
using Profitable.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Data.Seeding
{
    public class MarketTypesSeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            var marketTypeRepository = new Repository<MarketType>(dbContext);

            var types = JsonConvert.DeserializeObject<List<string>>(await new StreamReader("MarketTypes.json").ReadToEndAsync());

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
