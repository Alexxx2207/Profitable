using AutoMapper;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Comments;
using Profitable.Models.ResponseModels.Comments;

namespace Profitable.Automapper
{
    public class CommentsMapper : Profile
    {
        public CommentsMapper()
        {
            CreateMap<AddCommentRequestModel, Comment>();
            CreateMap<UpdateCommentRequestModel, Comment>();
            CreateMap<Comment, CommentResponseModel>();
        }
    }
}
