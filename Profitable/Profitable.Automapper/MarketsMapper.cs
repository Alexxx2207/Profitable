using AutoMapper;
using Profitable.Models.EntityModels;
using Profitable.Models.ViewModels.Markets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Automapper
{
    public class MarketsMapper : Profile
    {
        public MarketsMapper()
        {
            CreateMap<FinancialInstrument, FinantialInstrumentViewModel>()
                .ForMember(destination => destination.ExchangeName, options =>
                {
                    options.MapFrom(source => source.Exchange.Name);
                })
                .ForMember(destination => destination.MarketType, options => 
                {
                    options.MapFrom(source => source.MarketType.Name);
                });
        }
    }
}
