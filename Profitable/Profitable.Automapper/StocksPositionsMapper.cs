namespace Profitable.Common.Automapper
{
	using AutoMapper;
	using Profitable.Models.EntityModels;
	using Profitable.Models.RequestModels.Positions.Stocks;
	using Profitable.Models.ResponseModels.Positions.Stocks;

	public class StocksPositionsMapper : Profile
	{
		public StocksPositionsMapper()
		{
			CreateMap<AddStocksPositionRequestModel, TradePosition>();

			CreateMap<AddStocksPositionRequestModel, StocksPosition>();

			CreateMap<TradePosition, StocksPositionResponseModel>();

			CreateMap<StocksPosition, StocksPositionResponseModel>()
			   .ForMember(
				   dest => dest.Guid,
				   opt => opt.Ignore());
		}
	}
}
