using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.ResponseModels.Futures
{
	public class FuturesContractsResponseModel
	{
		public string Guid { get; set; }

		public string Name { get; set; }

		public double TickSize { get; set; }

		public double TickValue { get; set; }

	}
}
