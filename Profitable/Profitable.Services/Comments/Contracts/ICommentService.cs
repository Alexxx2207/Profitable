using Profitable.Common;
using Profitable.Models.RequestModels.Comments;
using Profitable.Models.ResponseModels.Comments;

namespace Profitable.Services.Comments.Contracts
{
    public interface ICommentService
    {
        Task<CommentResponseModel> GetCommentAsync(Guid guid);

        Task<List<CommentResponseModel>> GetCommentsByPostAsync(Guid guid);

        Task<Result> AddCommentAsync(AddCommentRequestModel newComment);

        Task<Result> DeleteCommentAsync(Guid guid);

        Task<Result> UpdateCommentAsync(UpdateCommentRequestModel newComment);
    }
}
