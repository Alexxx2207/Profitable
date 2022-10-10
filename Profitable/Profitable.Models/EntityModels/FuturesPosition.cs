using Profitable.GlobalConstants;
using Profitable.Models.EntityModels.EntityBaseClass;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profitable.Models.EntityModels
{
	public class FuturesPosition : EntityBase
	{
		[ForeignKey("TradePosition")]
		public Guid TradePositionId { get; set; }

		public TradePosition TradePosition { get; set; }

		public PositionDirection Direction { get; set; }

		[ForeignKey("FuturesContract")]
		public Guid FuturesContractId { get; set; }

		public FuturesContract FuturesContract { get; set; }

	}
}