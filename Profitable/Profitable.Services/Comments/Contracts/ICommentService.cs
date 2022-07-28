using Profitable.Common;
using Profitable.Models.RequestModels.Comments;
using Profitable.Models.ResponseModels.Comments;

namespace Profitable.Services.Comments.Contracts
{
    public interface ICommentService
    {
        Task<CommentResponseModel> GetCommentAsync(string guid);

        Task<List<CommentResponseModel>> GetCommentsByPostAsync(string postGUID);

        Task<Result> AddCommentAsync(AddCommentRequestModel newComment);

        Task<Result> DeleteCommentAsync(string guid);

        Task<Result> UpdateCommentAsync(UpdateCommentRequestModel newComment);
    }
}
