using AutoMapper;
using Profitable.Models.EntityModels;
using Profitable.Models.ResponseModels.Positions;

namespace Profitable.Common.Automapper
{
	public class PositionsMapper : Profile
	{
		public PositionsMapper()
		{
			CreateMap<PositionsRecordList, UserPositionsRecordResponseModel>()
				.ForMember(dest => dest.LastUpdated, src => src.MapFrom(model => model.LastUpdated.ToString("f")));
		}
	}
}
