namespace Profitable.AdminPanel.Common.Constants
{
	using Profitable.AdminPanel.Common.Models;
	using Profitable.AdminPanel.Common.Services.Seeders;

	public class GlobalConstants
	{
		public static List<SeederChoice> SeedChoices = new List<SeederChoice>()
		{
			new SeederChoice()
			{
				ChoiceMessage = "Market Types",
				ChoiceNumber = 1,
				Seeder = new MarketTypesSeeder()
			},
			new SeederChoice()
			{
				ChoiceMessage = "Exchanges",
				ChoiceNumber = 2,
				Seeder = new ExchangesSeeder()
			},
			new SeederChoice()
			{
				ChoiceMessage = "Futures Information",
				ChoiceNumber = 3,
				Seeder = new FuturesSeeder()
			},
			new SeederChoice()
			{
				ChoiceMessage = "Ticker Symbols",
				ChoiceNumber = 4,
				Seeder = new FinantialInstrumentsSeeder()
			},
			new SeederChoice()
			{
				ChoiceMessage = "Books",
				ChoiceNumber = 5,
				Seeder = new BooksSeeder()
			},
			new SeederChoice()
			{
				ChoiceMessage = "COT Reported Instruments",
				ChoiceNumber = 6,
				Seeder = new COTReportsSeeder()
			},
			new SeederChoice()
			{
				ChoiceMessage = "Seed All Entities",
				ChoiceNumber = 7,
				Seeder = new AllSeedersSeeder()
			},
		};
	}
}
