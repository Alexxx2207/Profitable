using AutoMapper;
using Profitable.Models.EntityModels;
using Profitable.Models.ResponseModels.Like;

namespace Profitable.Common.Automapper
{
    public class LikesMapper : Profile
    {
        public LikesMapper()
        {
            CreateMap<Like, LikeResponseModel>();
        }
    }
}
