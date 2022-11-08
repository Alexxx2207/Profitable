using Profitable.Common.Enums;
using Profitable.Models.EntityModels.EntityBaseClass;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profitable.Models.EntityModels
{
	public class StocksPosition : EntityBase
	{
		[Required]
		public string Name { get; set; }

        [ForeignKey("TradePosition")]
		public Guid TradePositionId { get; set; }

		public TradePosition TradePosition { get; set; }

		public double BuyCommission { get; set; }

		public double SellCommission { get; set; }
	}
}
