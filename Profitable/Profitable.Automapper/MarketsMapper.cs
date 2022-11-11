namespace Profitable.Common.Automapper
{
	using AutoMapper;
	using Profitable.Models.EntityModels;
	using Profitable.Models.ResponseModels.Markets;

	public class MarketsMapper : Profile
	{
		public MarketsMapper()
		{
			CreateMap<FinancialInstrument, FinantialInstrumentShortResponseModel>();
			CreateMap<FinancialInstrument, FinantialInstrumentExtendedResponseModel>()
				.ForMember(destination => destination.ExchangeName, options =>
				{
					options.MapFrom(source => source.Exchange.Name);
				});
			CreateMap<MarketType, MarketTypeResponseModel>();
		}
	}
}
