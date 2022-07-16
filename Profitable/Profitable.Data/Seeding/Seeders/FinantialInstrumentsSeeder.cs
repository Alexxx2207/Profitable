using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Profitable.Data.Repository;
using Profitable.Data.Seeding.Seeders.Contracts;
using Profitable.Models.EntityModels;

namespace Profitable.Data.Seeding.Seeders
{
    public class FinantialInstrumentsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider = null)
        {
            var instrumentRepository = new Repository<FinancialInstrument>(dbContext);

            var json = JsonConvert.DeserializeObject<List<Instrument>>(await new StreamReader("DataToSeed/TicketSymbols.json").ReadToEndAsync());

            var currentEntries = dbContext.FinancialInstruments;

            foreach (var instrument in json)
            {
                if (!currentEntries.Any(e => e.TickerSymbol == instrument.Symbol))
                {
                    var exchange = dbContext.Exchanges.First(e => e.Name == instrument.Exchange);
                    var marketType = dbContext.MarketTypes.First(e => e.Name == instrument.Type);

                    var finInstrument = new FinancialInstrument();
                    finInstrument.TickerSymbol = instrument.Symbol.ToUpper();
                    finInstrument.Exchange = exchange;
                    finInstrument.MarketType = marketType;

                    await instrumentRepository.AddAsync(finInstrument);
                }
            }

            await instrumentRepository.SaveChangesAsync();
        }

        private class Instrument
        {
            public string Symbol { get; set; }

            public string Type { get; set; }

            public string Exchange { get; set; }
        }
    }
}
