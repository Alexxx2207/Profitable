using Profitable.Common;
using Profitable.Models.InputModels.Comments;
using Profitable.Models.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Services.Comments.Contracts
{
    public interface ICommentService
    {
        Task<CommentViewModel> GetComment(string guid);

        Task<List<CommentViewModel>> GetCommentsByPost(string postGUID);

        Task<Result> AddComment(AddCommentInputModel newComment);

        Task<Result> DeleteComment(string guid);

        Task<Result> UpdateComment(UpdateCommentInputModel newComment);
    }
}
