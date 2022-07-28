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
        private readonly IRepository<FinancialInstrument> instrumentsRepository;
        private readonly IRepository<MarketType> marketTypesRepository;
        private readonly IMapper mapper;

        public MarketsService(
            IRepository<FinancialInstrument> instrumentsRepository,
            IRepository<MarketType> marketTypesRepository,
            IMapper mapper)
        {
            this.instrumentsRepository = instrumentsRepository;
            this.marketTypesRepository = marketTypesRepository;
            this.mapper = mapper;
        }

        public async Task<List<FinantialInstrumentShortViewModel>> GetAllFinantialInstrumentsAsync()
        {
            var instrument = await instrumentsRepository
                .GetAllAsNoTracking()
                .Select(instrument => mapper.Map<FinantialInstrumentShortViewModel>(instrument))
                .ToListAsync();

            return instrument;
        }

        public async Task<List<MarketTypeViewModel>> GetAllMarketTypesAsync()
        {
            var marketTypes = await marketTypesRepository
                .GetAllAsNoTracking()
                .Select(marketType => mapper.Map<MarketTypeViewModel>(marketType))
                .ToListAsync();

            return marketTypes;
        }

        public async Task<FinantialInstrumentExtendedViewModel> GetFinantialInstrumentBySymbolAsync(string symbol)
        {
            var instrument = await instrumentsRepository
                .GetAllAsNoTracking()
                .Include(i => i.Exchange)
                .FirstAsync(entity => entity.TickerSymbol == symbol.ToUpper());

            return mapper.Map<FinantialInstrumentExtendedViewModel>(instrument);
        }

        public async Task<List<FinantialInstrumentShortViewModel>> GetFinantialInstrumentsByTypeAsync(string type)
        {
            var instruments = await instrumentsRepository
                .GetAllAsNoTracking()
                .Include(i => i.MarketType)
                .Where(i => i.MarketType.Name == type)
                .Select(i => mapper.Map<FinantialInstrumentShortViewModel>(i))
                .ToListAsync();

            return instruments;
        }
    }
}
