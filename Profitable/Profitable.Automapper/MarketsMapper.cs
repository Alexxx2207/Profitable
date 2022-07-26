using AutoMapper;
using Profitable.Models.EntityModels;
using Profitable.Models.ViewModels.Markets;

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
