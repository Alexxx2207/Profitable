using Profitable.Common.Enums;
using Profitable.Models.ResponseModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Services.Positions.Contracts
{
	public interface IPositionsRecordsService
	{
		Task<List<UserPositionsRecordResponseModel>> GetUserPositionsRecordsAsync(
			Guid userGuid,
			int page,
			int pageCount,
			OrderPositionsRecordBy orderByChoice);
	}
}
