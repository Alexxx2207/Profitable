using AutoMapper;
using Profitable.Models.EntityModels;
using Profitable.Models.InputModels.Comments;
using Profitable.Models.ViewModels.Comments;

namespace Profitable.Automapper
{
    public class CommentsMapper : Profile
    {
        public CommentsMapper()
        {
            CreateMap<AddCommentInputModel, Comment>();
            CreateMap<UpdateCommentInputModel, Comment>();
            CreateMap<Comment, CommentViewModel>();
        }
    }
}
