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

        public async Task<Result> AddPostAsync(ApplicationUser author, AddPostRequestModel newPost)
        {
            try
            {
                var postToAdd = new Post()
                {
                    Title = newPost.Title,
                    Content = newPost.Content,
                    PostedOn = DateTime.UtcNow,
                    Author = author,

                };

                if (!string.IsNullOrWhiteSpace(newPost.ImageFileName))
                {
                    string newFileName = await GlobalServicesConstants.SaveUploadedImageAsync(ImageFor.Posts, newPost.ImageFileName, newPost.Image);
                    postToAdd.ImageURL = newFileName;
                }

                await postsRepository.AddAsync(postToAdd);

                await postsRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return "Post was not found!";
            }

        }

        public async Task<Result> DeletePostAsync(Guid guid)
        {
            try
            {
                var entity = await postsRepository
                .GetAllAsNoTracking()
                .FirstAsync(entity => entity.Guid == guid);

                postsRepository.Delete(entity);

                await postsRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return "Post was not found!";
            }

        }

        public async Task<Result> DeleteLikeAsync(Guid postGuid, Guid traderId)
        {
            var like = await likesRepository
                .GetAllAsNoTracking()
                .FirstAsync(entity => entity.PostId == postGuid);

            likesRepository.Delete(like);

            await likesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<PostResponseModel> GetPostByGuidAsync(Guid guid)
        {
            return mapper.Map<PostResponseModel>(await postsRepository
                .GetAllAsNoTracking()
                .Where(post => !post.IsDeleted)
                .Include(p => p.Tags)
                .Include(p => p.Author)
                .FirstAsync(entity => entity.Guid == guid));
        }

        public async Task<List<LikeResponseModel>> GetPostLikesAsync(Guid guid)
        {
            var likes = await likesRepository
                .GetAllAsNoTracking()
                .Where(post => !post.IsDeleted)
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
                    .Where(post => !post.IsDeleted)
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
                .Where(post => !post.IsDeleted)
                .Where(post => post.AuthorId == traderId)
                .Include(p => p.Tags)
                .Select(post => mapper.Map<PostResponseModel>(post))
                .ToListAsync();

            return posts;
        }

        public async Task<Result> UpdatePostAsync(string postToUpdateGuid, UpdatePostRequestModel newPost)
        {
            var postToUpdateGuidCasted = Guid.Parse(postToUpdateGuid);

            var postToUpdate = await postsRepository
                .GetAll()
                .Where(post => !post.IsDeleted)
                .FirstOrDefaultAsync(post => post.Guid == postToUpdateGuidCasted);


            if (postToUpdate != null)
            {
                string newFileName = await GlobalServicesConstants.SaveUploadedImageAsync(ImageFor.Posts, newPost.ImageFileName, newPost.Image);

                postToUpdate.Title = newPost.Title;
                postToUpdate.Content = newPost.Content;
                postToUpdate.ImageURL = newFileName;

                postsRepository.Update(postToUpdate);

                await postsRepository.SaveChangesAsync();

                return true;
            }
            else
            {
                return "Post was not found!";
            }
        }
    }
}
