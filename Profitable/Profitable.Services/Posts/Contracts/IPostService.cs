using Profitable.Common;
using Profitable.Models.RequestModels.Posts;
using Profitable.Models.ResponseModels.Like;
using Profitable.Models.ResponseModels.Posts;

namespace Profitable.Services.Posts.Contracts
{
    public interface IPostService
    {
        Task<PostResponseModel> GetPostByGuidAsync(string guid);

        Task<List<PostResponseModel>> GetPostsByPageAsync(int page);

        Task<List<PostResponseModel>> GetPostsByTraderAsync(string traderId);

        Task<List<LikeResponseModel>> GetPostLikesAsync(string guid);

        Task<Result> AddPostAsync(AddPostRequestModel newPost);

        Task<Result> DeletePostAsync(string guid);

        Task<Result> DeleteLikeAsync(string postGuid, string traderGuid);

        Task<Result> UpdatePostAsync(UpdatePostRequestModel newPost);
    }
}
