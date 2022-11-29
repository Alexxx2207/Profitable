namespace Profitable.Models.EntityModels
{
	using Profitable.Models.EntityModels.EntityBaseClass;
	using System.ComponentModel.DataAnnotations;

	public class List : EntityBase
	{
		public List()
		{
			FinancialInstruments = new HashSet<ListsFinancialInstruments>();
		}

		[Required]
		public string Name { get; set; }

		[Required]
		public Guid TraderId { get; set; }
		public ApplicationUser Trader { get; set; }

		public ICollection<ListsFinancialInstruments> FinancialInstruments { get; set; }

	}
}
