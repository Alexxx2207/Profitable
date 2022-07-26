using Profitable.Common;
using Profitable.Models.InputModels.Posts;
using Profitable.Models.ViewModels.Like;
using Profitable.Models.ViewModels.Posts;

namespace Profitable.Services.Posts.Contracts
{
    public interface IPostService
    {
        Task<PostViewModel> GetPostAsync(string guid);

        Task<List<PostViewModel>> GetPostsAsync(int page);

        Task<List<PostViewModel>> GetPostsByTraderAsync(string traderId);

        Task<List<LikeViewModel>> GetPostLikesAsync(string guid);

        Task<Result> AddPostAsync(AddPostInputModel newPost);

        Task<Result> DeletePostAsync(string guid);

        Task<Result> DeleteLikeAsync(string postGuid, string traderGuid);

        Task<Result> UpdatePost(UpdatePostInputModel newPost);
    }
}
