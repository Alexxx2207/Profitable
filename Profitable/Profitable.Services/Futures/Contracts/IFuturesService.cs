using Profitable.Models.ResponseModels.Futures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Services.Futures.Contracts
{
	public interface IFuturesService
	{
		Task<List<FuturesContractsResponseModel>> GetAllFuturesContracts();
	}
}
