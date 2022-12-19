namespace Profitable.AdminPanel.Services.Menu.Contracts
{
	using Profitable.AdminPanel.Common.Models;

	public interface IConsoleMenuService
	{
		void PrintSeedChoicesMenu(List<SeederChoice> SeedChoices);

		void ExecuteSeedChoice(int choice);
	}
}
