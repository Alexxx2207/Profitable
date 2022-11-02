using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common.Enums;
using Profitable.Common.GlobalConstants;
using Profitable.Common.Models;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Posts;
using Profitable.Models.ResponseModels.Posts;
using Profitable.Services.Common.Images.Contracts;
using Profitable.Services.Posts.Contracts;

namespace Profitable.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> postsRepository;
        private readonly IRepository<Like> likesRepository;
        private readonly IRepository<ApplicationUser> applicationUserRepository;
        private readonly IMapper mapper;
        private readonly IImageService imageService;

        public PostService(
            IRepository<Post> postsRepository,
            IRepository<Like> likesRepository,
            IRepository<ApplicationUser> applicationUserRepository,
            IMapper mapper,
            IImageService imageService)
        {
            this.postsRepository = postsRepository;
            this.likesRepository = likesRepository;
            this.applicationUserRepository = applicationUserRepository;
            this.mapper = mapper;
            this.imageService = imageService;
        }

        public async Task<Result> AddPostAsync(
            Guid authorId,
            AddPostRequestModel newPost)
        {
            if (newPost.Content.Length > GlobalServicesConstants.PostMaxLength)
            {
                throw new ArgumentException(
                    $"Content must be no longer than " +
                    $"{GlobalServicesConstants.PostMaxLength} characters.");
            }

            try
            {
                var postToAdd = mapper.Map<Post>(newPost, opt => 
                opt.AfterMap((src, dest) => 
                {
                    dest.AuthorId = authorId;
                    dest.PostedOn = DateTime.UtcNow;
                }));

                if (!string.IsNullOrWhiteSpace(newPost.ImageFileName))
                {
                    string newFileName =
                        await imageService.SaveUploadedImageAsync(
                            ImageFor.Posts,
                            newPost.ImageFileName,
                            newPost.Image);
                    postToAdd.ImageURL = newFileName;
                }

                await postsRepository.AddAsync(postToAdd);

                await postsRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<Result> DeletePostAsync(Guid guid, Guid requesterGuid)
        {
            try
            {
                var post = await postsRepository
                 .GetAllAsNoTracking()
                 .FirstOrDefaultAsync(entity => entity.Guid == guid);

                if (post == null)
                {
                    return GlobalServicesConstants.EntityDoesNotExist("Post");
                }

                if (post.AuthorId != requesterGuid)
                {
                    return GlobalServicesConstants.RequesterNotOwnerMesssage;
                }

                postsRepository.Delete(post);

                await postsRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<PostResponseModel> GetPostByGuidAsync(
            Guid guid,
            string? loggedInUserEmail)
        {
            var post = await postsRepository
                .GetAllAsNoTracking()
                .Include(p => p.Author)
                .Include(p => p.Likes)
                .Include("Likes.Author")
                .Where(post => !post.IsDeleted)
                .FirstOrDefaultAsync(entity => entity.Guid == guid);

            if(post == null)
            {
                throw new Exception(GlobalServicesConstants.EntityDoesNotExist("Post"));
            }
                
            var mappedPostResponseModel = mapper.Map<PostResponseModel>(post);

            if (loggedInUserEmail != null &&
                mappedPostResponseModel.Likes.Any(like => like.AuthorEmail == loggedInUserEmail))
            {
                mappedPostResponseModel.IsLikedByTheUsed = true;
            }

            return mappedPostResponseModel;
        }

        public async Task<List<PostResponseModel>> GetPostsByPageAsync(
            int page,
            int pageCount,
            string? loggedInUserEmail)
        {
            if (pageCount > 0 && pageCount <= GlobalServicesConstants.PostsMaxCountInPage)
            {
                var posts = await postsRepository
                    .GetAllAsNoTracking()
                    .Include(p => p.Author)
                    .Include(p => p.Likes)
                    .Include("Likes.Author")
                    .Include(p => p.Comments)
                    .Where(post => !post.IsDeleted)
                    .OrderByDescending(p => p.PostedOn)
                    .Skip(page * pageCount)
                    .Take(pageCount)
                    .Select(post => mapper.Map<PostResponseModel>(post))
                    .ToListAsync();

                foreach (var post in posts)
                {
                    if (loggedInUserEmail != null &&
                        post.Likes.Any(like => like.AuthorEmail == loggedInUserEmail))
                    {
                        post.IsLikedByTheUsed = true;
                    }
                }

                return posts;
            }
            else
            {
                throw new Exception(
                    $"Pages must be between 1 and {GlobalServicesConstants.PostsMaxCountInPage}!");
            }
        }

        public async Task<List<PostResponseModel>> GetPostsByUserAsync(
            Guid userId,
            int page,
            int pageCount)
        {
            var userEmail = (await applicationUserRepository
                .GetAllAsNoTracking()
                .FirstOrDefaultAsync(user => user.Id == userId))?.Email;

            if (userEmail == null)
            {
                throw new Exception(GlobalServicesConstants.EntityDoesNotExist("User"));
            }

            if (pageCount > 0 && pageCount <= GlobalServicesConstants.PostsMaxCountInPage)
            {
                var posts = await postsRepository
                    .GetAllAsNoTracking()
                    .Include(p => p.Author)
                    .Include(p => p.Likes)
                    .Include("Likes.Author")
                    .Include(p => p.Comments)
                    .Where(post =>
                        post.AuthorId == userId &&
                        !post.IsDeleted)
                    .OrderByDescending(p => p.PostedOn)
                    .Skip(page * pageCount)
                    .Take(pageCount)
                    .Select(post => mapper.Map<PostResponseModel>(post))
                    .ToListAsync();

                foreach (var post in posts)
                {
                    if (userEmail != null && post.Likes.Any(like => like.AuthorEmail == userEmail))
                    {
                        post.IsLikedByTheUsed = true;
                    }
                }

                return posts;
            }
            else
            {
                throw new Exception(
                    $"Pages must be between 1 and {GlobalServicesConstants.PostsMaxCountInPage}!");
            }
        }

        public async Task<Result> UpdatePostAsync(Guid postGuid, UpdatePostRequestModel newPost, Guid requesterGuid)
        {
            try
            {
                if (newPost.Content.Length > GlobalServicesConstants.PostMaxLength)
                {
                    throw new ArgumentException(
                        $"Content must be no longer than {GlobalServicesConstants.PostMaxLength} characters.");
                }

                var postToUpdate = await postsRepository
                    .GetAll()
                    .Where(post => !post.IsDeleted)
                    .FirstOrDefaultAsync(post => post.Guid == postGuid);

                if (postToUpdate == null)
                {
                    return GlobalServicesConstants.EntityDoesNotExist("Post");
                }

                if (postToUpdate.AuthorId != requesterGuid)
                {
                    return GlobalServicesConstants.RequesterNotOwnerMesssage;
                }

                await imageService.DeleteUploadedImageAsync(ImageFor.Posts, postToUpdate.ImageURL);

                string newFileName = "";

                if (!string.IsNullOrWhiteSpace(newPost.ImageFileName))
                {
                    newFileName =
                        await imageService.SaveUploadedImageAsync(
                            ImageFor.Posts, newPost.ImageFileName,
                            newPost.Image);
                }

                postToUpdate.Title = newPost.Title;
                postToUpdate.Content = newPost.Content;
                postToUpdate.ImageURL = newFileName;

                postsRepository.Update(postToUpdate);

                await postsRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<int> ManagePostLikeAsync(Guid authorId, Guid postGuid)
        {
            var postToUpdate = await postsRepository
                 .GetAllAsNoTracking()
                 .Where(post => !post.IsDeleted)
                 .Include(post => post.Likes)
                 .FirstOrDefaultAsync(post => post.Guid == postGuid);

            var postLikeExisted = await likesRepository
                .GetAllAsNoTracking()
                .FirstOrDefaultAsync(postLike =>
                    postLike.PostId == postGuid && postLike.AuthorId == authorId);

            int likesToShow = postToUpdate.Likes.Count;

            if (postLikeExisted == null)
            {
                var likeToAdd = new Like()
                {
                    PostId = postGuid,
                    AuthorId = authorId,
                };

                await likesRepository.AddAsync(likeToAdd);
                likesToShow++;
            }
            else
            {
                likesRepository.HardDelete(postLikeExisted);
                likesToShow--;
            }

            await likesRepository.SaveChangesAsync();

            return likesToShow;
        }
    }
}
