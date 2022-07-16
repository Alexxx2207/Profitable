using Newtonsoft.Json;
using Profitable.Data.Repository;
using Profitable.Data.Seeding.Seeders.Contracts;
using Profitable.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Data.Seeding.Seeders
{
    public class ExchangesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider = null)
        {
            var exchangeRepository = new Repository<Exchange>(dbContext);

            var types = JsonConvert.DeserializeObject<List<string>>(await new StreamReader("DataToSeed/Exchanges.json").ReadToEndAsync());

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
