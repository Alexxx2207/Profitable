namespace Profitable.AdminPanel.Common.Models
{
	using Profitable.AdminPanel.Common.Services.Seeders.Contracts;

	public class SeederChoice
	{
		public int ChoiceNumber { get; set; }

		public ISeeder Seeder { get; set; }

		public string ChoiceMessage { get; set; }
	}
}
