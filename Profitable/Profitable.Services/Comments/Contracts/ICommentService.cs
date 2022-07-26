using Profitable.Common;
using Profitable.Models.InputModels.Comments;
using Profitable.Models.ViewModels.Comments;

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
