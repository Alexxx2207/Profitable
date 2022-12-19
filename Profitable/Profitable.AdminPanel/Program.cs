using Profitable.AdminPanel.Common.Constants;
using Profitable.AdminPanel.Services.Menu;



namespace Profitable.AdminPanel
{
	public class Program
	{
		static void Main(string[] args)
		{
			string choice;

			var dbContext = GlobalConfiguration.CreateDbContext();

			var consoleMenuService = new ConsoleMenuService(dbContext);

			consoleMenuService.PrintSeedChoicesMenu(GlobalConstants.SeedChoices);

			while ((choice = Console.ReadLine()) != (GlobalConstants.SeedChoices.Count + 1).ToString())
			{
				if (int.TryParse(choice, out int choiceInt))
				{
					consoleMenuService.ExecuteSeedChoice(choiceInt);
					Console.WriteLine("\nDone!\n");
				}

				consoleMenuService.PrintSeedChoicesMenu(GlobalConstants.SeedChoices);
			}
		}
	}
}