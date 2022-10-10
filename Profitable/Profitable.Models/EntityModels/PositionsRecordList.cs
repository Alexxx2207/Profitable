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
	public class PositionsRecordList : EntityBase
	{
		public PositionsRecordList()
		{
			Positions = new HashSet<TradePosition>();
		}

		[Required]
		public string Name { get; set; }


		[ForeignKey("User")]
		public Guid UserId { get; set; }

		public ApplicationUser User { get; set; }

		public ICollection<TradePosition> Positions { get; set; }
	}
}
