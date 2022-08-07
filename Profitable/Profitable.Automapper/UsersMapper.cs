using AutoMapper;
using Profitable.Automapper.TypeConverters;
using Profitable.GlobalConstants;
using Profitable.Models.EntityModels;
using Profitable.Models.ResponseModels.Users;

namespace Profitable.Automapper
{
    public class UsersMapper : Profile
    {
        public UsersMapper()
        {
            CreateMap<ApplicationUser, UserDetailsResponseModel>()
                .ForMember(
                    dest => dest.Guid,
                    opt => opt.MapFrom(src => src.Id)
                )
                .ForMember(
                dest => dest.ProfileImage,
                opt => opt.ConvertUsing(new ImageByteArrayConverter(ImageFor.Users), src => src.ProfilePictureURL));

        }
    }
}
