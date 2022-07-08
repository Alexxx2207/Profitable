using Profitable.Models.InputModels.Posts;
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
        Task<List<PostViewModel>> GetRecentPosts(int postsCount);

        Task<PostViewModel> GetPost(string guid);

        Task<List<PostViewModel>> GetPostsByTrader(string traderId);

        Task<List<LikeViewModel>> GetPostLikes(string guid);

        Task AddPost(AddPostInputModel newPost);

        Task DeletePost(string guid);

        Task UpdatePost(UpdatePostInputModel newPost);
    }
}
