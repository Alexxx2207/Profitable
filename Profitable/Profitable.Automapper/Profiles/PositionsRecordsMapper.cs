namespace Profitable.Common.Automapper.Profiles
{
    using AutoMapper;
    using Profitable.Models.EntityModels;
    using Profitable.Models.ResponseModels.Positions.Records;

    public class PositionsRecordsMapper : Profile
    {
        public PositionsRecordsMapper()
        {
            CreateMap<PositionsRecordList, UserPositionsRecordResponseModel>()
                .ForMember(
                    dest => dest.InstrumentGroup,
                    src => src.MapFrom(model => model.InstrumentGroup.ToString()));
        }
    }
}
