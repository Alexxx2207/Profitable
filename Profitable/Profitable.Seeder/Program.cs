using Profitable.Data.Seeding;

Console.WriteLine("What to seed: ");

bool done = false;
while (!done)
{
    Console.WriteLine(@"
        Seed:
        0. Exit seeding
        1. Market Types
        2. Exchanges
        3. Finantial Instruments
    ");

    byte choice = byte.Parse(Console.ReadLine());

    switch (choice)
    {
        case 0:
            done = true;
            break;        
        case 1:
            var seederMarketTypes = new MarketTypesSeeder();
            await seederMarketTypes.SeedAsync(new Profitable.Data.ApplicationDbContext());
            break; 
        case 2:
            var seederExchanges = new ExchangesSeeder();
            await seederExchanges.SeedAsync(new Profitable.Data.ApplicationDbContext());
            break; 
        case 3:
            var seederFinantialInstruments = new FinantialInstrumentsSeeder();
            await seederFinantialInstruments.SeedAsync(new Profitable.Data.ApplicationDbContext());
            break;
        default:
            break;
    }
}
