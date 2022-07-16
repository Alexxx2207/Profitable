using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.InputModels.Comments;
using Profitable.Models.ViewModels.Comments;
using Profitable.Services.Comments.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Result> AddComment(AddCommentInputModel newComment)
        {
            var comment = mapper.Map<Comment>(newComment);

            await repository.AddAsync(comment);

            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteComment(string guid)
        {
            var comment = await repository
                .GetAllAsNoTracking()
                .FirstAsync(entity => entity.GUID.ToString() == guid);

            repository.Delete(comment);

            await repository.SaveChangesAsync();

            return true;
        }
        
        public async Task<CommentViewModel> GetComment(string guid)
        {
            var comment = await repository
                .GetAllAsNoTracking()
                .FirstAsync(entity => entity.GUID.ToString() == guid);

            return mapper.Map<CommentViewModel>(comment);
        }

        public async Task<List<CommentViewModel>> GetCommentsByPost(string postGUID)
        {
            var comments = await repository
                .GetAllAsNoTracking()
                .Where(comment => comment.PostId == postGUID)
                .ToListAsync();

            return comments
                .Select(comment => mapper.Map<CommentViewModel>(comment))
                .ToList();
        }

        public async Task<Result> UpdateComment(UpdateCommentInputModel newComment)
        {
            var comment = mapper.Map<Comment>(newComment);

            var existingComment = await repository
                .GetAll().
                FirstAsync(entity => entity.GUID.ToString() == newComment.Guid);

            if(existingComment == null)
            {
                return GlobalConstants.GlobalServicesConstants.EntityDoesNotExist;
            }

            existingComment.Content = newComment.Content;

            await repository.SaveChangesAsync();

            return true;
        }
    }
}
