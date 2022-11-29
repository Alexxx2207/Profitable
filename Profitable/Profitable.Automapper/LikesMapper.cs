namespace Profitable.Common.Automapper
{
	using AutoMapper;
	using Profitable.Models.EntityModels;
	using Profitable.Models.ResponseModels.Like;

	public class LikesMapper : Profile
	{
		public LikesMapper()
		{
			CreateMap<Like, LikeResponseModel>()
				.ForMember(
				dest => dest.AuthorEmail,
				opt => opt.MapFrom(src => src.Author.Email));
		}
	}
}
