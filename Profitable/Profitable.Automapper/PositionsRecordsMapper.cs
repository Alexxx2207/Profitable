using AutoMapper;
using Profitable.Models.EntityModels;
using Profitable.Models.ResponseModels.Positions.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Common.Automapper
{
    public class PositionsRecordsMapper : Profile
    {
        public PositionsRecordsMapper()
        {
            CreateMap<PositionsRecordList, UserPositionsRecordResponseModel>()
                .ForMember(
                    dest => dest.LastUpdated,
                    src => src.MapFrom(model => model.LastUpdated.ToString("f")))
                .ForMember(
                    dest => dest.InstrumentGroup,
                    src => src.MapFrom(model => model.InstrumentGroup.ToString()));
        }
    }
}
