namespace Profitable.Services.COT.Contracts
{
	using Profitable.Models.RequestModels.COT;
	using Profitable.Models.ResponseModels.COT;

	public interface ICOTService
	{
		Task<GetCOTResponseModel> GetReport(GetCOTRequestModel getCOTRequestModel);
	}
}
