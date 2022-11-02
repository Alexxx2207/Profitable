using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common.GlobalConstants;
using Profitable.Common.Models;
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

		public async Task<Result> AddCommentAsync(
            Guid postGuid,
			AddCommentRequestModel postRequestModel,
			Guid requesterGuid)
		{
			try
			{
                if (postRequestModel.Content.Length > GlobalServicesConstants.CommentMaxLength)
                {
                    return
                        $"Content must be no longer than {GlobalServicesConstants.CommentMaxLength} characters.";
                }

                var newComment = mapper.Map<Comment>(postRequestModel, opt =>
                opt.AfterMap((src, dest) =>
                {
                    dest.PostId = postGuid;
                    dest.AuthorId = requesterGuid;
                    dest.PostedOn = DateTime.UtcNow;

                }));

                await repository.AddAsync(newComment);

                await repository.SaveChangesAsync();

                return true;
            }
			catch (Exception e)
			{
                return e.Message;
            }
        }

		public async Task<Result> DeleteCommentAsync(Guid guid, Guid requesterGuid)
		{
			try
			{
				var comment = await repository
				.GetAllAsNoTracking()
				.FirstOrDefaultAsync(entity => entity.Guid == guid);

				if(comment == null)
				{
					return "Comment not found";
				}

				if(comment.AuthorId != requesterGuid)
				{
					return GlobalServicesConstants.RequesterNotOwnerMesssage;
                }

				repository.Delete(comment);

				await repository.SaveChangesAsync();

				return true;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public async Task<List<CommentResponseModel>> GetCommentsByPostAsync(
			Guid guid,
			int page,
			int pageCount)
		{
			var comments = await repository
				.GetAllAsNoTracking()
				.Where(comment =>
					comment.IsDeleted == false &&
					comment.PostId == guid)
				.OrderByDescending(c => c.PostedOn)
				.Skip(page * pageCount)
				.Take(pageCount)
				.Include(c => c.Author)
				.Select(comment => mapper.Map<CommentResponseModel>(comment))
				.ToListAsync();

			return comments;
		}

		public async Task<List<CommentResponseModel>> GetCommentsByUserAsync(
			Guid userGuid,
			int page,
			int pageCount)
		{
			var comments = await repository
				.GetAllAsNoTracking()
				.Where(comment =>
					comment.IsDeleted == false &&
					comment.AuthorId == userGuid)
				.OrderByDescending(c => c.PostedOn)
				.Skip(page * pageCount)
				.Take(pageCount)
				.Include(c => c.Author)
				.Select(comment => mapper.Map<CommentResponseModel>(comment))
				.ToListAsync();

			return comments;
		}

		public async Task<int> GetCommentsCountByPostAsync(Guid guid)
		{
			var a = (await repository
				.GetAllAsNoTracking()
				.Where(comment =>
					comment.PostId == guid &&
					comment.IsDeleted == false)
				.ToListAsync()).Count;

			var comments = (
				await repository
				.GetAllAsNoTracking()
				.Where(comment =>
					comment.PostId == guid &&
					comment.IsDeleted == false)
				.Select(comment => mapper.Map<CommentResponseModel>(comment))
				.ToListAsync()
				)
				.Count;

			return comments;
		}

		public async Task<Result> UpdateCommentAsync(
			Guid guid,
			UpdateCommentRequestModel newComment,
			Guid requesterGuid)
		{
			if (newComment.Content.Length > GlobalServicesConstants.CommentMaxLength)
			{
				return
					$"Content must be no longer than " +
					$"{GlobalServicesConstants.CommentMaxLength} characters.";
			}

			try
			{
                var comment = mapper.Map<Comment>(newComment);

                var existingComment = await repository
                    .GetAll().
                    FirstAsync(entity => entity.Guid == guid);

                if (existingComment == null)
                {
                    return GlobalServicesConstants.EntityDoesNotExist("Comment");
                }

                if (existingComment.AuthorId != requesterGuid)
                {
                    return GlobalServicesConstants.RequesterNotOwnerMesssage;
                }

                existingComment.Content = newComment.Content;

                await repository.SaveChangesAsync();

                return true;
            }
			catch (Exception e)
			{
				return e.Message;
			}
		}
	}
}
