using Profitable.Models.ViewModels.Markets;

namespace Profitable.Services.Markets.Contract
{
    public interface IMarketsService
    {
        Task<List<FinantialInstrumentViewModel>> GetAllFinantialInstruments();

        Task<FinantialInstrumentViewModel> GetFinantialInstrumentBySymbol(string symbol);
    }
}
