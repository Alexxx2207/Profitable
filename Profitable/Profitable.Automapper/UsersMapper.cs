using AutoMapper;
using Profitable.Automapper.TypeConverters;
using Profitable.GlobalConstants;
using Profitable.Models.EntityModels;
using Profitable.Models.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Automapper
{
    public class UsersMapper : Profile
    {
        public UsersMapper()
        {
            CreateMap<ApplicationUser, UserDetailsViewModel>()
                .ForMember(
                    dest => dest.GUID,
                    opt => opt.MapFrom(src => src.Id)
                )
                .ForMember(
                    dest => dest.ImageType,
                    opt => opt.MapFrom(src => Enum.GetName(typeof(ImageTypes), src))
                )
                .ForMember(
                dest => dest.ProfileImage,
                opt => opt.ConvertUsing(new ImageByteArrayConverter(ImageFor.ProfilePic), src => src.ProfilePictureURL));
        }
    }
}
