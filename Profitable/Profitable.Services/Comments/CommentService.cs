using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Comments;
using Profitable.Models.ResponseModels.Comments;
using Profitable.Services.Comments.Contracts;

namespace Profitable.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> repository;
        private readonly IMapper mapper;

        public CommentService(IRepository<Comment> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result> AddCommentAsync(AddCommentRequestModel newComment)
        {
            var comment = mapper.Map<Comment>(newComment);

            await repository.AddAsync(comment);

            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteCommentAsync(string guid)
        {
            var comment = await repository
                .GetAllAsNoTracking()
                .FirstAsync(entity => entity.GUID.ToString() == guid);

            repository.Delete(comment);

            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<CommentResponseModel> GetCommentAsync(string guid)
        {
            var comment = await repository
                .GetAllAsNoTracking()
                .FirstAsync(entity => entity.GUID.ToString() == guid);

            return mapper.Map<CommentResponseModel>(comment);
        }

        public async Task<List<CommentResponseModel>> GetCommentsByPostAsync(string postGUID)
        {
            var comments = await repository
                .GetAllAsNoTracking()
                .Where(comment => comment.PostId == postGUID)
                .Select(comment => mapper.Map<CommentResponseModel>(comment))
                .ToListAsync();

            return comments;
        }

        public async Task<Result> UpdateCommentAsync(UpdateCommentRequestModel newComment)
        {
            var comment = mapper.Map<Comment>(newComment);

            var existingComment = await repository
                .GetAll().
                FirstAsync(entity => entity.GUID.ToString() == newComment.Guid);

            if (existingComment == null)
            {
                return GlobalConstants.GlobalServicesConstants.EntityDoesNotExist;
            }

            existingComment.Content = newComment.Content;

            await repository.SaveChangesAsync();

            return true;
        }
    }
}
