using Profitable.Common.Enums;
using Profitable.Common.Models;
using Profitable.Models.ResponseModels.Positions.Records;

namespace Profitable.Services.Positions.Contracts
{
	public interface IPositionsRecordsService
	{
		Task<List<UserPositionsRecordResponseModel>> GetUserRecordsAsync(
			Guid userGuid,
			int page,
			int pageCount,
			OrderPositionsRecordBy orderByChoice);

		Task<List<UserPositionsRecordResponseModel>> GetUserRecordsByInstrumentGroupAsync(
			Guid userGuid,
			int page,
			int pageCount,
			InstrumentGroup instrumentGroup);

		IEnumerable<string> GetPositionsRecordsOrderTypes();

		Task<Result> AddPositionsRecordList(Guid userGuid, string recordName, string instrumentGroup);

		Task<Result> ChangeNamePositionsRecordList(Guid recordGuid, string recordName, Guid requesterGuid);

		Task<Result> DeletePositionsRecordList(Guid recordGuid, Guid requesterGuid);
	}
}
