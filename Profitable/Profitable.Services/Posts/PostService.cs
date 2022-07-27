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

        public async Task<Result> AddPostAsync(AddPostInputModel newPost)
        {
            var postToAdd = mapper.Map<Post>(newPost);

            postToAdd.PostedOn = DateTime.UtcNow;

            await postsRepository.AddAsync(postToAdd);

            await postsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeletePostAsync(string guid)
        {
            var entity = await postsRepository.GetAllAsNoTracking().FirstAsync(entity => entity.GUID.ToString() == guid);

            postsRepository.Delete(entity);

            await postsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteLikeAsync(string postGuid, string traderId)
        {
            var like = await likesRepository
                .GetAllAsNoTracking()
                .FirstAsync(entity => entity.PostId == postGuid && entity.TraderId == traderId);

            likesRepository.Delete(like);

            await likesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<PostViewModel> GetPostAsync(string guid)
        {
            return mapper.Map<PostViewModel>(await postsRepository
                .GetAllAsNoTracking()
                .Include(p => p.Tags)
                .FirstAsync(entity => entity.GUID == guid));
        }

        public async Task<List<LikeViewModel>> GetPostLikesAsync(string guid)
        {
            var likes = await likesRepository
                .GetAllAsNoTracking()
                .Where(like => like.PostId == guid)
                .Select(like => mapper.Map<LikeViewModel>(like))
                .ToListAsync();

            return likes;
        }

        public async Task<List<PostViewModel>> GetPostsByPageAsync(int page)
        {
            var posts = await postsRepository
                .GetAllAsNoTracking()
                .OrderByDescending(p => p.PostedOn)
                .Skip(page * GlobalServicesConstants.PostsCountInPage)
                .Take(GlobalServicesConstants.PostsCountInPage)
                .Include(p => p.Tags)
                .Include(p => p.Author)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .Select(post => mapper.Map<PostViewModel>(post))
                .ToListAsync();

            return posts;
        }

        public async Task<List<PostViewModel>> GetPostsByTraderAsync(string traderId)
        {
            var posts = await postsRepository
                .GetAllAsNoTracking()
                .Where(post => post.AuthorId == traderId)
                .Include(p => p.Tags)
                .Select(post => mapper.Map<PostViewModel>(post))
                .ToListAsync();

            return posts;
        }

        public async Task<Result> UpdatePostAsync(UpdatePostInputModel newPost)
        {
            var post = await postsRepository
                .GetAll()
                .FirstAsync(entity => entity.GUID.ToString() == newPost.GUID);

            var existingPost = await postsRepository
               .GetAll()
               .FirstAsync(entity => entity.GUID.ToString() == newPost.GUID);

            if (existingPost == null)
            {
                return GlobalServicesConstants.EntityDoesNotExist;
            }

            existingPost.Title = newPost.Title;
            existingPost.Content = newPost.Content;

            await postsRepository.SaveChangesAsync();

            return true;
        }
    }
}
