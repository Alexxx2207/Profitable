namespace Profitable.Common.Automapper.Profiles
{
    using AutoMapper;
    using Profitable.Models.EntityModels;
    using Profitable.Models.RequestModels.Positions.Futures;
    using Profitable.Models.ResponseModels.Positions.Futures;

    public class FuturesPositionsMapper : Profile
    {
        public FuturesPositionsMapper()
        {
            CreateMap<AddFuturesPositionRequestModel, TradePosition>()
                .ForMember(
                    dest => dest.QuantitySize,
                    src => src.MapFrom(model => model.Quantity));

            CreateMap<TradePosition, FuturesPositionResponseModel>();

            CreateMap<FuturesContract, FuturesPositionResponseModel>()
                 .ForMember(
                    dest => dest.ContractName,
                    opt => opt.MapFrom(model => model.Name))
                 .ForMember(
                    dest => dest.Guid,
                    opt => opt.Ignore());

        }
    }
}
