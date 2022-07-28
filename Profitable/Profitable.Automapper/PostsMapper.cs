using AutoMapper;
using Profitable.Automapper.TypeConverters;
using Profitable.GlobalConstants;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Posts;
using Profitable.Models.ResponseModels.Posts;

namespace Profitable.Automapper
{
    public class PostsMapper : Profile
    {
        public PostsMapper()
        {
            CreateMap<AddPostRequestModel, Post>();
            CreateMap<UpdatePostRequestModel, Post>();
            CreateMap<Post, PostResponseModel>()
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
