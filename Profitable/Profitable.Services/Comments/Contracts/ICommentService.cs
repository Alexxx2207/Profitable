using Profitable.Common;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Comments;
using Profitable.Models.ResponseModels.Comments;

namespace Profitable.Services.Comments.Contracts
{
    public interface ICommentService
    {
        Task<List<CommentResponseModel>> GetCommentsByPostAsync(Guid guid, int page, int pageCount);

        Task<int> GetCommentsCountByPostAsync(Guid guid);

        Task<List<CommentResponseModel>> GetCommentsByUserAsync(Guid userGuid, int page, int pageCount);

        Task<Result> AddCommentAsync(Comment newComment);

        Task<Result> DeleteCommentAsync(Guid guid);

        Task<Result> UpdateCommentAsync(Guid guid, UpdateCommentRequestModel newComment);
    }
}
