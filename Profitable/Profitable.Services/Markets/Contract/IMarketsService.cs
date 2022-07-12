using Profitable.Models.ViewModels.Markets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Services.Markets.Contract
{
    public interface IMarketsService
    {
        Task<List<FinantialInstrumentViewModel>> GetAllFinantialInstruments();

        Task<FinantialInstrumentViewModel> GetFinantialInstrumentBySymbol(string symbol);
    }
}
