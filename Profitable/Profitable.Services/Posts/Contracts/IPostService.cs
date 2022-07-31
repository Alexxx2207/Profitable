using Profitable.Common;
using Profitable.Models.RequestModels.Posts;
using Profitable.Models.ResponseModels.Like;
using Profitable.Models.ResponseModels.Posts;

namespace Profitable.Services.Posts.Contracts
{
    public interface IPostService
    {
        Task<PostResponseModel> GetPostByGuidAsync(Guid guid);

        Task<List<PostResponseModel>> GetPostsByPageAsync(int page, int postsCount);

        Task<List<PostResponseModel>> GetPostsByTraderAsync(Guid traderId);

        Task<List<LikeResponseModel>> GetPostLikesAsync(Guid guid);

        Task<Result> AddPostAsync(AddPostRequestModel newPost);

        Task<Result> DeletePostAsync(Guid guid);

        Task<Result> DeleteLikeAsync(Guid postGuid, Guid traderGuid);

        Task<Result> UpdatePostAsync(UpdatePostRequestModel newPost);
    }
}
