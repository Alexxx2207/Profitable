namespace Profitable.Services.Positions.Contracts
{
	using Profitable.Common.Models;
	using Profitable.Models.RequestModels.Positions.Futures;
	using Profitable.Models.ResponseModels.Positions.Futures;
	public interface IFuturesPositionsService
	{
		Task<List<FuturesPositionResponseModel>> GetFuturesPositions(Guid recordId, DateTime afterDateFilter, DateTime beforeDateFilter);

		Task<FuturesPositionResponseModel> GetFuturesPositionByGuid(Guid positionGuid);

		Task<Result> AddFuturesPositions(Guid recordId, AddFuturesPositionRequestModel model, Guid requesterGuid);

		Task<Result> ChangeFuturesPosition(
			Guid recordId,
			Guid positionGuid,
			Guid requesterGuid,
			ChangeFuturesPositionRequestModel model);

		Task<Result> DeleteFuturesPositions(Guid recordId, Guid positionGuid, Guid requesterGuid);

		CalculateFuturesPositionResponseModel CalculateFuturesPosition(CalculateFuturesPositionRequestModel model);
	}
}
