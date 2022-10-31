using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common.Enums;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.ResponseModels.Markets;
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

        public async Task<List<FinantialInstrumentShortResponseModel>> GetAllFinantialInstrumentsAsync()
        {
            var instrument = await instrumentsRepository
                .GetAllAsNoTracking()
                .Select(instrument => mapper.Map<FinantialInstrumentShortResponseModel>(instrument))
                .ToListAsync();

            return instrument;
        }

        public IEnumerable<string> GetAllInstrumentGroupsAsync()
        {
            return Enum.GetValues<InstrumentGroup>().Select(group => group.ToString());
        }

        public async Task<List<MarketTypeResponseModel>> GetAllMarketTypesAsync()
        {
            var marketTypes = await marketTypesRepository
                .GetAllAsNoTracking()
                .Select(marketType => mapper.Map<MarketTypeResponseModel>(marketType))
                .ToListAsync();

            return marketTypes;
        }

        public async Task<FinantialInstrumentExtendedResponseModel> GetFinantialInstrumentBySymbolAsync(string symbol)
        {
            var instrument = await instrumentsRepository
                .GetAllAsNoTracking()
                .Include(i => i.Exchange)
                .FirstOrDefaultAsync(entity => entity.TickerSymbol == symbol);

            if(instrument == null)
            {
                throw new Exception("Instrument not found");
            }

            return mapper.Map<FinantialInstrumentExtendedResponseModel>(instrument);
        }

        public async Task<List<FinantialInstrumentShortResponseModel>> GetFinantialInstrumentsByTypeAsync(string type)
        {
            var instruments = await instrumentsRepository
                .GetAllAsNoTracking()
                .Include(i => i.MarketType)
                .Where(i => i.MarketType.Name == type)
                .Select(i => mapper.Map<FinantialInstrumentShortResponseModel>(i))
                .ToListAsync();

            return instruments;
        }
    }
}
