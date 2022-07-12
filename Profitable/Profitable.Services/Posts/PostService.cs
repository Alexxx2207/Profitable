using AutoMapper;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.InputModels.Posts;
using Profitable.Models.ViewModels.Like;
using Profitable.Models.ViewModels.Posts;
using Profitable.Services.Posts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> postsRepository;
        private readonly IRepository<Like> likesRepository;
        private readonly IMapper mapper;

        public PostService(IRepository<Post> postsRepository, IRepository<Like> likesRepository, IMapper mapper)
        {
            this.postsRepository = postsRepository;
            this.likesRepository = likesRepository;
            this.mapper = mapper;
        }

        public async Task AddPost(AddPostInputModel newPost)
        {
            postsRepository.Add(mapper.Map<Post>(newPost));
        }

        public async Task DeletePost(string guid)
        {
            postsRepository.Delete(await postsRepository.GetAsync(guid));
        }

        public async Task<PostViewModel> GetPost(string guid)
        {
            return mapper.Map<PostViewModel>(await postsRepository.GetAsync(guid));
        }

        public async Task<List<LikeViewModel>> GetPostLikes(string guid)
        {
            var likes = await likesRepository.FindAllWhere(like => like.PostId ==  guid);

            return likes
                .Select(like => mapper.Map<LikeViewModel>(like))
                .ToList();
        }

        public async Task<List<PostViewModel>> GetPosts(int skip, int take)
        {
            var posts = await postsRepository.GetAllAsync(skip, take);

            return posts.Select(post => mapper.Map<PostViewModel>(post)).ToList();
        }

        public async Task<List<PostViewModel>> GetPostsByTrader(string traderId)
        {
            var posts = await postsRepository.FindAllWhere(post => post.AuthorId == traderId);

            return posts
                .Select(post => mapper.Map<PostViewModel>(post))
                .ToList();
        }

        public async Task<List<PostViewModel>> GetRecentPosts(int postsCount)
        {
            var posts = await postsRepository.GetAllAsync();

            return posts
                .Select(post => mapper.Map<PostViewModel>(post))
                .Take(postsCount)
                .ToList();
        }

        public async Task UpdatePost(UpdatePostInputModel newPost)
        {
            var post = await postsRepository.GetAsync(newPost.GUID);

            await postsRepository.UpdateAsync(post);
        }
    }
}
