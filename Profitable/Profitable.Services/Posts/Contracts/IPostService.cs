using Profitable.Common;
using Profitable.Models.InputModels.Posts;
using Profitable.Models.ViewModels.Like;
using Profitable.Models.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Services.Posts.Contracts
{
    public interface IPostService
    {
        Task<List<PostViewModel>> GetRecentPosts();

        Task<PostViewModel> GetPost(string guid);

        Task<List<PostViewModel>> GetPostsByTrader(string traderId);

        Task<List<LikeViewModel>> GetPostLikes(string guid);

        Task<Result> AddPost(AddPostInputModel newPost);

        Task<Result> DeletePost(string guid);

        Task<Result> DeleteLike(string postGuid, string traderGuid);

        Task<Result> UpdatePost(UpdatePostInputModel newPost);
    }
}
