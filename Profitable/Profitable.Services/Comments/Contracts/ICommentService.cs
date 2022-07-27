using Profitable.Common;
using Profitable.Models.InputModels.Comments;
using Profitable.Models.ViewModels.Comments;

namespace Profitable.Services.Comments.Contracts
{
    public interface ICommentService
    {
        Task<CommentViewModel> GetCommentAsync(string guid);

        Task<List<CommentViewModel>> GetCommentsByPostAsync(string postGUID);

        Task<Result> AddCommentAsync(AddCommentInputModel newComment);

        Task<Result> DeleteCommentAsync(string guid);

        Task<Result> UpdateCommentAsync(UpdateCommentInputModel newComment);
    }
}
