namespace Profitable.Services.Futures.Contracts
{
	using Profitable.Models.ResponseModels.Futures;
	public interface IFuturesService
	{
		Task<List<FuturesContractsResponseModel>> GetAllFuturesContracts();
	}
}
