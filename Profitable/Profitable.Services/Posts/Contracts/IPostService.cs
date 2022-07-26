using Profitable.Common;
using Profitable.Models.InputModels.Posts;
using Profitable.Models.ViewModels.Like;
using Profitable.Models.ViewModels.Posts;

namespace Profitable.Services.Posts.Contracts
{
    public interface IPostService
    {
        Task<PostViewModel> GetPost(string guid);

        public Task<List<PostViewModel>> GetPosts(int page);

        Task<List<PostViewModel>> GetPostsByTrader(string traderId);

        Task<List<LikeViewModel>> GetPostLikes(string guid);

        Task<Result> AddPost(AddPostInputModel newPost);

        Task<Result> DeletePost(string guid);

        Task<Result> DeleteLike(string postGuid, string traderGuid);

        Task<Result> UpdatePost(UpdatePostInputModel newPost);
    }
}
