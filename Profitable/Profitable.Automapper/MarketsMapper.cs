using AutoMapper;
using Profitable.Models.EntityModels;
using Profitable.Models.ViewModels.Markets;

namespace Profitable.Automapper
{
    public class MarketsMapper : Profile
    {
        public MarketsMapper()
        {
            CreateMap<FinancialInstrument, FinantialInstrumentShortViewModel>();
            CreateMap<FinancialInstrument, FinantialInstrumentExtendedViewModel>()
                .ForMember(destination => destination.ExchangeName, options =>
                {
                    options.MapFrom(source => source.Exchange.Name);
                });
            CreateMap<MarketType, MarketTypeViewModel>();
        }
    }
}
