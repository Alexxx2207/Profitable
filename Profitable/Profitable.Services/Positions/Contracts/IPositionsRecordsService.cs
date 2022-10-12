using Profitable.Common.Enums;
using Profitable.Common.Models;
using Profitable.Models.ResponseModels.Positions;

namespace Profitable.Services.Positions.Contracts
{
	public interface IPositionsRecordsService
	{
		Task<List<UserPositionsRecordResponseModel>> GetUserPositionsRecordsAsync(
			Guid userGuid,
			int page,
			int pageCount,
			OrderPositionsRecordBy orderByChoice);

		IEnumerable<string> GetPositionsRecordsOrderTypes();

		Task<Result> AddPositionsRecordList(Guid userGuid, string positionName);
	}
}
