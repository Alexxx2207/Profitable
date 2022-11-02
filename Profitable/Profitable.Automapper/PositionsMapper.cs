using AutoMapper;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Positions;
using Profitable.Models.ResponseModels.Positions;

namespace Profitable.Common.Automapper
{
	public class PositionsMapper : Profile
	{
		public PositionsMapper()
		{
			CreateMap<PositionsRecordList, UserPositionsRecordResponseModel>()
				.ForMember(
					dest => dest.LastUpdated,
					src => src.MapFrom(model => model.LastUpdated.ToString("f")))
				.ForMember(
					dest => dest.InstrumentGroup,
					src => src.MapFrom(model => model.InstrumentGroup.ToString()));

			CreateMap<AddFuturesPositionRequestModel, TradePosition>()
				.ForMember(
					dest => dest.QuantitySize,
					src => src.MapFrom(model => model.Quantity));

            CreateMap<TradePosition, PositionResponseModel>()
                .ForMember(
					dest => dest.Quantity,
                    opt => opt.MapFrom(model => model.QuantitySize))
				.ForMember(
					dest => dest.PositionAddedOn,
                    opt => opt.MapFrom(model => model.PositionAddedOn.ToString("F")))
				.ForMember(
					dest => dest.PositionPAndL,
					opt => opt.MapFrom(model => model.RealizedProfitAndLoss));

            CreateMap<FuturesContract, PositionResponseModel>()
                 .ForMember(
                    dest => dest.ContractName,
                    opt => opt.MapFrom(model => model.Name))
				 .ForMember(
                    dest => dest.Guid,
                    opt => opt.Ignore());

        }
	}
}
