namespace Profitable.Common.Automapper.Profiles
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
			CreateMap<RegisterUserRequestModel, ApplicationUser>()
				.ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
			CreateMap<ApplicationUser, UserDetailsResponseModel>()
				.ForMember(
					dest => dest.Guid,
					opt => opt.MapFrom(src => src.Guid))
				.ForMember(
					dest => dest.OrganizationId,
					opt => opt.MapFrom(src => src.OrganizationId))
				.ForMember(
					dest => dest.OrganizationRole,
					opt => opt.MapFrom(src => src.OrganizationRole.ToString()))
				.ForMember(
				dest => dest.ProfileImage,
				opt => opt.ConvertUsing(
					new ImageByteArrayConverter(ImageFor.Users),
					src => src.ProfilePictureURL));
			CreateMap<ApplicationUser, AuthUserModel>()
				.ForMember(
					dest => dest.Guid,
					opt => opt.MapFrom(src => src.Guid));
		}
	}
}
