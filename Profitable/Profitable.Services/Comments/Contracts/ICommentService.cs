using Profitable.Common.Models;
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

        Task<Result> AddCommentAsync(Guid postGuid, AddCommentRequestModel postRequestModel, Guid requesterGuid);

        Task<Result> DeleteCommentAsync(Guid guid, Guid requesterGuid);

        Task<Result> UpdateCommentAsync(Guid guid, UpdateCommentRequestModel newComment, Guid requesterGuid);
    }
}
