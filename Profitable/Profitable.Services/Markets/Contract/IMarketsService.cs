using Profitable.Models.ViewModels.Markets;

namespace Profitable.Services.Markets.Contract
{
    public interface IMarketsService
    {
        Task<List<FinantialInstrumentViewModel>> GetAllFinantialInstrumentsAsync();

        Task<FinantialInstrumentViewModel> GetFinantialInstrumentBySymbolAsync(string symbol);
    }
}
