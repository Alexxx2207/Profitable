using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.ResponseModels.Positions
{
	public class OrderPositionsRecordsByTypes
	{
		public IEnumerable<string> Types { get; set; }
	}
}
