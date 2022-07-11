using AutoMapper;
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

        public async Task AddComment(AddCommentInputModel newComment)
        {
            var comment = mapper.Map<Comment>(newComment);

            repository.Add(comment);
        }

        public async Task DeleteComment(string guid)
        {
            var comment = await repository.GetAsync(guid);

            repository.Delete(comment);
        }

        public async Task<CommentViewModel> GetComment(string guid)
        {
            var comment = await repository.GetAsync(guid);

            return mapper.Map<CommentViewModel>(comment);
        }

        public async Task<List<CommentViewModel>> GetCommentsByPost(string postGUID)
        {
            var comments = await repository.FindAllWhere(comment => comment.PostId == postGUID);

            return comments
                .Select(comment => mapper.Map<CommentViewModel>(comment))
                .ToList();
        }

        public async Task UpdateComment(UpdateCommentInputModel newComment)
        {
            var comment = mapper.Map<Comment>(newComment);

            await repository.UpdateAsync(comment);
        }
    }
}
