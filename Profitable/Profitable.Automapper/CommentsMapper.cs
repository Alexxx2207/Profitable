using AutoMapper;
using Profitable.Models;
using Profitable.Models.InputModels.Comments;
using Profitable.Models.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
