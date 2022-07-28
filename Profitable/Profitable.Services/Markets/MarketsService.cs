using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.ViewModels.Markets;
using Profitable.Services.Markets.Contract;

namespace Profitable.Services.Markets
{
    public class MarketsService : IMarketsService
    {
        private readonly IRepository<FinancialInstrument> repository;
        private readonly IMapper mapper;

        public MarketsService(IRepository<FinancialInstrument> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<List<FinantialInstrumentViewModel>> GetAllFinantialInstrumentsAsync()
        {
            var instrument = await repository
                .GetAllAsNoTracking()
                .Select(instrument => mapper.Map<FinantialInstrumentViewModel>(instrument))
                .ToListAsync();

            return instrument;
        }

        public async Task<FinantialInstrumentViewModel> GetFinantialInstrumentBySymbolAsync(string symbol)
        {
            var instrument = await repository
                .GetAllAsNoTracking()
                .Include(i => i.Exchange)
                .FirstAsync(entity => entity.TickerSymbol == symbol.ToUpper());

            return mapper.Map<FinantialInstrumentViewModel>(instrument);
        }

        public async Task<List<FinantialInstrumentViewModel>> GetFinantialInstrumentsByType(string type)
        {
            var instruments = await repository
                .GetAllAsNoTracking()
                .Include(i => i.MarketType)
                .Where(i => i.MarketType.Name == type)
                .Select(i => mapper.Map<FinantialInstrumentViewModel>(i))
                .ToListAsync();

            return instruments;
        }
    }
}
