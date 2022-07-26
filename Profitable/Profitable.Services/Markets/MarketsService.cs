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

        public async Task<List<FinantialInstrumentViewModel>> GetAllFinantialInstruments()
        {
            var instrument = await repository
                .GetAllAsNoTracking()
                .ToListAsync();

            return instrument
                .Select(instrument => mapper.Map<FinantialInstrumentViewModel>(instrument))
                .ToList();
        }

        public async Task<FinantialInstrumentViewModel> GetFinantialInstrumentBySymbol(string symbol)
        {
            var instrument = await repository
                .GetAllAsNoTracking()
                .Include(i => i.Exchange)
                .Include(i => i.MarketType)
                .FirstAsync(entity => entity.TickerSymbol == symbol.ToUpper());

            return mapper.Map<FinantialInstrumentViewModel>(instrument);
        }
    }
}
