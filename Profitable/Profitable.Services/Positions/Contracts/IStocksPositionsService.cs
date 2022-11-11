namespace Profitable.Services.Positions.Contracts
{
	using Profitable.Common.Models;
	using Profitable.Models.RequestModels.Positions.Stocks;
	using Profitable.Models.ResponseModels.Positions.Stocks;

	public interface IStocksPositionsService
	{
		Task<List<StocksPositionResponseModel>> GetStocksPositions(Guid recordId, DateTime afterDateFilter, DateTime beforeDateFilter);

		Task<StocksPositionResponseModel> GetStocksPositionByGuid(Guid positionGuid);

		Task<Result> AddStocksPositions(Guid recordId, AddStocksPositionRequestModel model, Guid requesterGuid);

		Task<Result> ChangeStocksPosition(
			Guid recordId,
			Guid positionGuid,
			Guid requesterGuid,
			ChangeStocksPositionRequestModel model);

		Task<Result> DeleteStocksPositions(Guid recordId, Guid positionGuid, Guid requesterGuid);

		CalculateStocksPositionResponseModel CalculateStocksPosition(CalculateStocksPositionRequestModel model);
	}
}
