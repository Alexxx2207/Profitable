using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.ResponseModels.Users
{
	public class UserPositionsRecordResponseModel
	{
		public string Guid { get; set; }

		public string CreatedOn { get; set; }

		public int PositionsCount { get; set; }
	}

}
