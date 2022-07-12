using AutoMapper;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.ViewModels.Markets;
using Profitable.Services.Markets.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var instrument = (await repository.GetAllAsync()).ToList();

            return instrument.Select(instrument => mapper.Map<FinantialInstrumentViewModel>(instrument)).ToList();
        }

        public Task<FinantialInstrumentViewModel> GetFinantialInstrumentBySymbol(string symbol)
        {
            var instrument = (await repository.FindAllWhere()).ToList();
        }
    }
}
