namespace Profitable.AdminPanel.Services.Menu
{
	using Profitable.AdminPanel.Common.Constants;
	using Profitable.AdminPanel.Common.Models;
	using Profitable.AdminPanel.Services.Menu.Contracts;
	using Profitable.Data;

	public class ConsoleMenuService : IConsoleMenuService
	{
		private readonly ApplicationDbContext dbContext;

		public ConsoleMenuService(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public void ExecuteSeedChoice(int choice)
		{
			var seedChoice = GlobalConstants.SeedChoices.FirstOrDefault(x => x.ChoiceNumber == choice);

			seedChoice?.Seeder.SeedAsync(dbContext);
		}

		public void PrintSeedChoicesMenu(List<SeederChoice> seedChoices)
		{
			foreach (var choice in seedChoices)
			{
				Console.WriteLine($"{choice.ChoiceNumber}. {choice.ChoiceMessage}");
			}
			Console.WriteLine($"{seedChoices.Count + 1}. Exit");
			Console.Write("Choice: ");
		}
	}
}
