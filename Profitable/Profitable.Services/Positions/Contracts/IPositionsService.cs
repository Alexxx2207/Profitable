using Profitable.Common.Models;
using Profitable.Models.RequestModels.Positions;
using Profitable.Models.ResponseModels.Positions;

namespace Profitable.Services.Positions.Contracts
{
	public interface IPositionsService
	{
        Task<List<PositionResponseModel>> GetFuturesPositions(Guid recordId, DateTime afterDateFilter);

		Task<PositionResponseModel> GetFuturesPositionByGuid(Guid positionGuid);

		Task<Result> AddFuturesPositions(Guid recordId, AddFuturesPositionRequestModel model);

        Task<Result> ChangeFuturesPosition(Guid recordId, Guid positionGuid, Guid requesterGuid, ChangeFuturesPositionRequestModel model);

        Task<Result> DeleteFuturesPositions(Guid recordId, Guid positionGuid, Guid requesterGuid);

    }
}
