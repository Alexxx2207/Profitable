using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common.Models;
using Profitable.Data.Repository.Contract;
using Profitable.Common.GlobalConstants;
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

		public async Task<Result> AddCommentAsync(Comment newComment)
		{
			if (newComment.Content.Length > GlobalServicesConstants.CommentMaxLength)
			{
				return $"Content must be no longer than {GlobalServicesConstants.CommentMaxLength} characters.";
			}

			await repository.AddAsync(newComment);

			await repository.SaveChangesAsync();

			return true;
		}

		public async Task<Result> DeleteCommentAsync(Guid guid)
		{
			try
			{
				var comment = await repository
				.GetAllAsNoTracking()
				.FirstAsync(entity => entity.Guid == guid);

				repository.Delete(comment);

				await repository.SaveChangesAsync();

				return true;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public async Task<List<CommentResponseModel>> GetCommentsByPostAsync(Guid guid, int page, int pageCount)
		{
			var comments = await repository
				.GetAllAsNoTracking()
				.Where(comment => comment.IsDeleted == false)
				.Where(comment => comment.PostId == guid)
				.OrderByDescending(c => c.PostedOn)
				.Skip(page * pageCount)
				.Take(pageCount)
				.Include(c => c.Author)
				.Select(comment => mapper.Map<CommentResponseModel>(comment))
				.ToListAsync();

			return comments;
		}

		public async Task<List<CommentResponseModel>> GetCommentsByUserAsync(Guid userGuid, int page, int pageCount)
		{
			var comments = await repository
				.GetAllAsNoTracking()
				.Where(comment => comment.IsDeleted == false)
				.Where(comment => comment.AuthorId == userGuid)
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
				.Where(comment => comment.PostId == guid)
				.Where(comment => comment.IsDeleted == false)
				.ToListAsync()).Count;

			var comments = (
				await repository
				.GetAllAsNoTracking()
				.Where(comment => comment.PostId == guid)
				.Where(comment => comment.IsDeleted == false)
				.Select(comment => mapper.Map<CommentResponseModel>(comment))
				.ToListAsync()
				)
				.Count;

			return comments;
		}

		public async Task<Result> UpdateCommentAsync(Guid guid, UpdateCommentRequestModel newComment)
		{
			if (newComment.Content.Length > GlobalServicesConstants.CommentMaxLength)
			{
				return $"Content must be no longer than {GlobalServicesConstants.CommentMaxLength} characters.";
			}

			var comment = mapper.Map<Comment>(newComment);

			var existingComment = await repository
				.GetAll().
				FirstAsync(entity => entity.Guid == guid);

			if (existingComment == null)
			{
				return GlobalServicesConstants.EntityDoesNotExist;
			}

			existingComment.Content = newComment.Content;

			await repository.SaveChangesAsync();

			return true;
		}
	}
}
