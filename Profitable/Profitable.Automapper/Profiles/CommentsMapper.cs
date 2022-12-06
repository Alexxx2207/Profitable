﻿namespace Profitable.Common.Automapper.Profiles
{
    using AutoMapper;
    using Profitable.Models.EntityModels;
    using Profitable.Models.RequestModels.Comments;
    using Profitable.Models.ResponseModels.Comments;

    public class CommentsMapper : Profile
    {
        public CommentsMapper()
        {
            CreateMap<AddCommentRequestModel, Comment>();
            CreateMap<UpdateCommentRequestModel, Comment>();
            CreateMap<Comment, CommentResponseModel>()
                .ForMember(
                    dest => dest.AuthorName,
                    src =>
                    {
                        src.MapFrom(comment =>
                        $"{comment.Author.FirstName} {comment.Author.LastName}");
                    }
                 )
                .ForMember(
                    dest => dest.AuthorEmail,
                    src =>
                    {
                        src.MapFrom(comment => comment.Author.Email);
                    }
                );
        }
    }
}