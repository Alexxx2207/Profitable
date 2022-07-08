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

        Task AddComment(AddCommentInputModel newComment);

        Task DeleteComment(string guid);

        Task UpdateComment(UpdateCommentInputModel newComment);
    }
}
