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
	}
}
