namespace Profitable.Services.Markets.Contract
{
	using Profitable.Models.ResponseModels.Markets;

	public interface IMarketsService
	{
		Task<List<FinantialInstrumentShortResponseModel>> GetAllFinantialInstrumentsAsync();

		Task<List<MarketTypeResponseModel>> GetAllMarketTypesAsync();

		IEnumerable<string> GetAllInstrumentGroups();

		Task<List<FinantialInstrumentShortResponseModel>> GetFinantialInstrumentsByTypeAsync(string type);

		Task<FinantialInstrumentExtendedResponseModel> GetFinantialInstrumentBySymbolAsync(string symbol);

	}
}
