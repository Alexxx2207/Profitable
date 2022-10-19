using Profitable.Models.EntityModels.EntityBaseClass;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profitable.Models.EntityModels
{
	public class TradePosition : EntityBase
	{
		public TradePosition()
		{
			PositionAddedOn = DateTime.UtcNow;
		}

		public DateTime PositionAddedOn { get; set; }

		public double QuantitySize { get; set; }

		[ForeignKey("PositionsRecordList")]
		public Guid PositionsRecordListId { get; set; }

		public PositionsRecordList PositionsRecordList { get; set; }

		[Required]
		public double EntryPrice { get; set; }

		[Required]
		public double ExitPrice { get; set; }

		public double RealizedProfitAndLoss { get; set; }
	}
}
