namespace Profitable.Common.Automapper
{
	using AutoMapper;
	using Profitable.Automapper.TypeConverters;
	using Profitable.Common.Enums;
	using Profitable.Common.Models;
	using Profitable.Models.EntityModels;
	using Profitable.Models.RequestModels.Users;
	using Profitable.Models.ResponseModels.Users;

	public class UsersMapper : Profile
	{
		public UsersMapper()
		{
			CreateMap<ApplicationUser, UserDetailsResponseModel>()
				.ForMember(
					dest => dest.Guid,
					opt => opt.MapFrom(src => src.Id))
				.ForMember(
				dest => dest.ProfileImage,
				opt => opt.ConvertUsing(
					new ImageByteArrayConverter(ImageFor.Users),
					src => src.ProfilePictureURL));
			CreateMap<ApplicationUser, AuthUserModel>()
				.ForMember(
					dest => dest.Guid,
					opt => opt.MapFrom(src => src.Id));
			CreateMap<RegisterUserRequestModel, ApplicationUser>()
				.ForMember(
					dest => dest.UserName,
					opt => opt.MapFrom(src => src.Email));
		}
	}
}
