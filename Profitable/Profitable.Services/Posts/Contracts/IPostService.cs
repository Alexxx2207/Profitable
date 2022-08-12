using Profitable.Common;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Posts;
using Profitable.Models.ResponseModels.Posts;

namespace Profitable.Services.Posts.Contracts
{
    public interface IPostService
    {
        Task<PostResponseModel> GetPostByGuidAsync(Guid guid);

        Task<List<PostResponseModel>> GetPostsByPageAsync(int page, int postsCount);

        Task<List<PostResponseModel>> GetPostsByTraderAsync(Guid traderId);

        Task<int> ManagePostLikeAsync(ApplicationUser author, string postGuid);

        Task<Result> AddPostAsync(ApplicationUser author, AddPostRequestModel newPost);

        Task<Result> DeletePostAsync(Guid guid);

        Task<Result> UpdatePostAsync(string postToUpdateGuid, UpdatePostRequestModel newPost);
    }
}
