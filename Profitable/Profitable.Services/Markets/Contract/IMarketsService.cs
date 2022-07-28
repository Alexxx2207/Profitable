using Profitable.Models.ViewModels.Markets;

namespace Profitable.Services.Markets.Contract
{
    public interface IMarketsService
    {
        Task<List<FinantialInstrumentShortViewModel>> GetAllFinantialInstrumentsAsync();

        Task<List<MarketTypeViewModel>> GetAllMarketTypesAsync();

        Task<List<FinantialInstrumentShortViewModel>> GetFinantialInstrumentsByTypeAsync(string type);

        Task<FinantialInstrumentExtendedViewModel> GetFinantialInstrumentBySymbolAsync(string symbol);

    }
}
