using Profitable.Common.Enums;
using Profitable.Models.EntityModels.EntityBaseClass;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profitable.Models.EntityModels
{
	public class PositionsRecordList : EntityBase
	{
		public PositionsRecordList()
		{
			Positions = new HashSet<TradePosition>();
			LastUpdated = DateTime.UtcNow;
		}

		[Required]
		public string Name { get; set; }

		public DateTime LastUpdated { get; set; }

		public InstrumentGroup InstrumentGroup { get; set; }


		[ForeignKey("User")]
		public Guid UserId { get; set; }

		public ApplicationUser User { get; set; }


		public ICollection<TradePosition> Positions { get; set; }
	}
}
