using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common;
using Profitable.Data.Repository.Contract;
using Profitable.GlobalConstants;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Posts;
using Profitable.Models.ResponseModels.Like;
using Profitable.Models.ResponseModels.Posts;
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

        public async Task<Result> AddPostAsync(AddPostRequestModel newPost)
        {
            var postToAdd = mapper.Map<Post>(newPost);

            postToAdd.PostedOn = DateTime.UtcNow;

            await postsRepository.AddAsync(postToAdd);

            await postsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeletePostAsync(Guid guid)
        {
            var entity = await postsRepository.GetAllAsNoTracking().FirstAsync(entity => entity.Guid == guid);

            postsRepository.Delete(entity);

            await postsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteLikeAsync(Guid postGuid, Guid traderId)
        {
            var like = await likesRepository
                .GetAllAsNoTracking()
                .FirstAsync(entity => entity.PostId == postGuid && entity.TraderId == traderId);

            likesRepository.Delete(like);

            await likesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<PostResponseModel> GetPostByGuidAsync(Guid guid)
        {
            return mapper.Map<PostResponseModel>(await postsRepository
                .GetAllAsNoTracking()
                .Include(p => p.Tags)
                .Include(p => p.Author)
                .FirstAsync(entity => entity.Guid == guid));
        }

        public async Task<List<LikeResponseModel>> GetPostLikesAsync(Guid guid)
        {
            var likes = await likesRepository
                .GetAllAsNoTracking()
                .Where(like => like.PostId == guid)
                .Select(like => mapper.Map<LikeResponseModel>(like))
                .ToListAsync();

            return likes;
        }

        public async Task<List<PostResponseModel>> GetPostsByPageAsync(int page, int postsCount)
        {
            if (postsCount > 0 && postsCount <= GlobalServicesConstants.PostsMaxCountInPage)
            {
                var posts = await postsRepository
                    .GetAllAsNoTracking()
                    .OrderByDescending(p => p.PostedOn)
                    .Skip(page * postsCount)
                    .Take(postsCount)
                    .Include(p => p.Tags)
                    .Include(p => p.Author)
                    .Include(p => p.Likes)
                    .Include(p => p.Comments)
                    .Select(post => mapper.Map<PostResponseModel>(post))
                    .ToListAsync();

                return posts;
            }
            else
            {
                throw new Exception($"Pages must be between 1 and {GlobalServicesConstants.PostsMaxCountInPage}!");
            }
        }

        public async Task<List<PostResponseModel>> GetPostsByTraderAsync(Guid traderId)
        {
            var posts = await postsRepository
                .GetAllAsNoTracking()
                .Where(post => post.AuthorId == traderId)
                .Include(p => p.Tags)
                .Select(post => mapper.Map<PostResponseModel>(post))
                .ToListAsync();

            return posts;
        }

        public async Task<Result> UpdatePostAsync(UpdatePostRequestModel newPost)
        {
            var post = await postsRepository
                .GetAll()
                .FirstAsync(entity => entity.Guid == Guid.Parse(newPost.Guid));

            var existingPost = await postsRepository
               .GetAll()
               .FirstAsync(entity => entity.Guid == Guid.Parse(newPost.Guid));

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
