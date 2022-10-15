using AutoMapper;
using Profitable.Automapper.TypeConverters;
using Profitable.Common.Enums;
using Profitable.Models.EntityModels;
using Profitable.Models.ResponseModels.Posts;

namespace Profitable.Common.Automapper
{
    public class PostsMapper : Profile
    {
        public PostsMapper()
        {
            CreateMap<Post, PostResponseModel>()
                .ForMember(
                    dest => dest.AuthorEmail,
                    opt => opt.MapFrom(source => source.Author.Email))
                .ForMember(
                    dest => dest.Author,
                    opt => opt.MapFrom(source => $"{source.Author.FirstName} {source.Author.LastName}"))
                .ForMember(
                    dest => dest.PostedOn,
                    opt => opt.MapFrom(source => source.PostedOn.ToString("D")))
                .ForMember(
                    dest => dest.PostImage,
                    opt => opt.ConvertUsing(new ImageByteArrayConverter(ImageFor.Posts), src => src.ImageURL))
                .ForMember(
                    dest => dest.AuthorImage,
                    opt => opt.ConvertUsing(new ImageByteArrayConverter(ImageFor.Users), src => src.Author.ProfilePictureURL));
        }
    }
}
