using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common;
using Profitable.Data.Repository.Contract;
using Profitable.GlobalConstants;
using Profitable.Models.EntityModels;
using Profitable.Models.InputModels.Posts;
using Profitable.Models.ViewModels.Like;
using Profitable.Models.ViewModels.Posts;
using Profitable.Services.Posts.Contracts;

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

        public async Task<Result> AddPost(AddPostInputModel newPost)
        {
            var postToAdd = mapper.Map<Post>(newPost);

            postToAdd.PostedOn = DateTime.UtcNow;

            await postsRepository.AddAsync(postToAdd);

            await postsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeletePost(string guid)
        {
            var entity = await postsRepository.GetAllAsNoTracking().FirstAsync(entity => entity.GUID.ToString() == guid);

            postsRepository.Delete(entity);

            await postsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteLike(string postGuid, string traderId)
        {
            var like = await likesRepository
                .GetAllAsNoTracking()
                .FirstAsync(entity => entity.PostId == postGuid && entity.TraderId == traderId);

            likesRepository.Delete(like);

            await likesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<PostViewModel> GetPost(string guid)
        {
            return mapper.Map<PostViewModel>(await postsRepository
                .GetAllAsNoTracking()
                .Include(p => p.Tags)
                .FirstAsync(entity => entity.GUID.ToString() == guid));
        }

        public async Task<List<LikeViewModel>> GetPostLikes(string guid)
        {
            var likes = await likesRepository
                .GetAllAsNoTracking()
                .Where(like => like.PostId == guid)
                .ToListAsync();

            return likes
                .Select(like => mapper.Map<LikeViewModel>(like))
                .ToList();
        }

        public async Task<List<PostViewModel>> GetPosts(int skip, int take)
        {
            var posts = await postsRepository
                .GetAllAsNoTracking()
                .Skip(skip)
                .Take(take)
                .Include(p => p.Tags)
                .ToListAsync();

            return posts.Select(post => mapper.Map<PostViewModel>(post)).ToList();
        }

        public async Task<List<PostViewModel>> GetPostsByTrader(string traderId)
        {
            var posts = await postsRepository
                .GetAllAsNoTracking()
                .Where(post => post.AuthorId == traderId)
                .Include(p => p.Tags)
                .ToListAsync();

            return posts
                .Select(post => mapper.Map<PostViewModel>(post))
                .ToList();
        }

        public async Task<List<PostViewModel>> GetRecentPosts()
        {
            var posts = await postsRepository
                .GetAllAsNoTracking()
                .Take(GlobalServicesConstants.RecentPostsCount)
                .Include(p => p.Tags)
                .ToListAsync();

            return posts
                .Select(post => mapper.Map<PostViewModel>(post))
                .ToList();
        }

        public async Task<Result> UpdatePost(UpdatePostInputModel newPost)
        {
            var post = await postsRepository
                .GetAll()
                .FirstAsync(entity => entity.GUID.ToString() == newPost.GUID);

            var existingPost = await postsRepository
               .GetAll()
               .FirstAsync(entity => entity.GUID.ToString() == newPost.GUID);

            if (existingPost == null)
            {
                return GlobalConstants.GlobalServicesConstants.EntityDoesNotExist;
            }

            existingPost.Title = newPost.Title;
            existingPost.Content = newPost.Content;

            await postsRepository.SaveChangesAsync();

            return true;
        }
    }
}
