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
    public class ExchangesSeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            var exchangeRepository = new Repository<Exchange>(dbContext, dbContext.Exchanges);

            var types = JsonConvert.DeserializeObject<List<string>>(await new StreamReader("Exchanges.json").ReadToEndAsync());

            var currentEntries = dbContext.Exchanges;

            foreach (var instrument in types.DistinctBy(type => type))
            {
                if (!currentEntries.Any(e => e.Name == instrument))
                {
                    var exchange = new Exchange();
                    exchange.Name = instrument;

                    await exchangeRepository.AddAsync(exchange);
                }
            }

            await exchangeRepository.SaveChangesAsync();
        }
    }
}
