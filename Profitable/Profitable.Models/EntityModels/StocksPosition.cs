using Profitable.Models.EntityModels.EntityBaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
