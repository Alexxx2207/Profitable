namespace Profitable.Models.EntityModels
{
	using Profitable.Models.EntityModels.EntityBaseClass;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class FinancialInstrument : EntityBase
	{
		[Required]
		public string TickerSymbol { get; set; }

		[Required]
		public Guid ExchangeId { get; set; }
		public Exchange Exchange { get; set; }

		[ForeignKey("MarketType")]
		public Guid MarketTypeId { get; set; }
		public MarketType MarketType { get; set; }
	}
}
