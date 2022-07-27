using AutoMapper;
using Profitable.Automapper.TypeConverters;
using Profitable.GlobalConstants;
using Profitable.Models.EntityModels;
using Profitable.Models.InputModels.Posts;
using Profitable.Models.ViewModels.Posts;

namespace Profitable.Automapper
{
    public class PostsMapper : Profile
    {
        public PostsMapper()
        {
            CreateMap<AddPostInputModel, Post>();
            CreateMap<UpdatePostInputModel, Post>();
            CreateMap<Post, PostViewModel>()
                .ForMember(
                    dest => dest.Author,
                    opt => opt.MapFrom(source => $"{source.Author.FirstName} {source.Author.LastName}"))
                .ForMember(
                    dest => dest.PostedOn,
                    opt => opt.MapFrom(source => source.PostedOn.ToString("f")))
                .ForMember(
                    dest => dest.PostImageType,
                    opt => opt.MapFrom(source => source.ImageType.ToString()))
                .ForMember(
                    dest => dest.PostImage,
                    opt => opt.ConvertUsing(new ImageByteArrayConverter(ImageFor.Posts), src => src.ImageURL))
                .ForMember(
                    dest => dest.AuthorImage,
                    opt => opt.ConvertUsing(new ImageByteArrayConverter(ImageFor.ProfilePic), src => src.Author.ProfilePictureURL))
                .ForMember(
                    dest => dest.AuthorImageType,
                    opt => opt.MapFrom(source => source.Author.ImageType.ToString()));
        }
    }
}
