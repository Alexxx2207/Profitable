using Profitable.Models.ResponseModels.Markets;

namespace Profitable.Services.Markets.Contract
{
    public interface IMarketsService
    {
        Task<List<FinantialInstrumentShortResponseModel>> GetAllFinantialInstrumentsAsync();

        Task<List<MarketTypeResponseModel>> GetAllMarketTypesAsync();

        Task<List<FinantialInstrumentShortResponseModel>> GetFinantialInstrumentsByTypeAsync(string type);

        Task<FinantialInstrumentExtendedResponseModel> GetFinantialInstrumentBySymbolAsync(string symbol);

    }
}
