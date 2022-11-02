using AutoMapper;
using Profitable.Automapper.TypeConverters;
using Profitable.Common.Enums;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Posts;
using Profitable.Models.ResponseModels.Posts;

namespace Profitable.Common.Automapper
{
    public class PostsMapper : Profile
    {
        public PostsMapper()
        {
            CreateMap<AddPostRequestModel, Post>();
            CreateMap<Post, PostResponseModel>()
                .ForMember(
                    dest => dest.AuthorEmail,
                    opt => opt.MapFrom(source => source.Author.Email))
                .ForMember(
                    dest => dest.Author,
                    opt => opt.MapFrom(source =>
                    $"{source.Author.FirstName} {source.Author.LastName}"))
                .ForMember(
                    dest => dest.PostedOn,
                    opt => opt.MapFrom(source => source.PostedOn.ToString("D")))
                .ForMember(
                    dest => dest.PostImage,
                    opt => opt.ConvertUsing(
                        new ImageByteArrayConverter(ImageFor.Posts),
                        src => src.ImageURL))
                .ForMember(
                    dest => dest.PostImageFileName,
                    opt => opt.MapFrom(source => source.ImageURL))
                .ForMember(
                    dest => dest.AuthorImage,
                    opt => opt.ConvertUsing(
                        new ImageByteArrayConverter(ImageFor.Users),
                        src => src.Author.ProfilePictureURL))
                .ForMember(
                    dest => dest.LikesCount,
                    opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(
                    dest => dest.IsLikedByTheUsed,
                    opt => opt.Ignore());
        }
    }
}
