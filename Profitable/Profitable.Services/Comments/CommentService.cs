using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Comments;
using Profitable.Models.ResponseModels.Comments;
using Profitable.Services.Comments.Contracts;
using System;

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
			await repository.AddAsync(newComment);

			await repository.SaveChangesAsync();

			return true;
		}

		public async Task<Result> DeleteCommentAsync(Guid guid)
		{
			var comment = await repository
				.GetAllAsNoTracking()
				.FirstAsync(entity => entity.Guid == guid);

			repository.Delete(comment);

			await repository.SaveChangesAsync();

			return true;
		}

		public async Task<List<CommentResponseModel>> GetCommentsByPostAsync(Guid guid, int page, int pageCount)
		{
			var comments = await repository
				.GetAllAsNoTracking()
				.Where(comment => comment.PostId == guid)
				.Skip(page * pageCount)
				.Take(pageCount)
				.OrderByDescending(c => c.PostedOn)
				.Include(c => c.Author)
				.Select(comment => mapper.Map<CommentResponseModel>(comment))
				.ToListAsync();

			return comments;
		}

		public async Task<List<CommentResponseModel>> GetCommentsByUserAsync(Guid userGuid, int page, int pageCount)
		{
			var comments = await repository
				.GetAllAsNoTracking()
				.Where(comment => comment.AuthorId == userGuid)
				.Skip(page * pageCount)
				.Take(pageCount)
				.OrderByDescending(c => c.PostedOn)
				.Include(c => c.Author)
				.Select(comment => mapper.Map<CommentResponseModel>(comment))
				.ToListAsync();

			return comments;
		}

		public async Task<int> GetCommentsCountByPostAsync(Guid guid)
		{
			var comments = await repository
				.GetAllAsNoTracking()
				.Where(comment => comment.PostId == guid)
				.Select(comment => mapper.Map<CommentResponseModel>(comment))
				.ToListAsync();

			return comments.Count;
		}

		public async Task<Result> UpdateCommentAsync(UpdateCommentRequestModel newComment)
		{
			var comment = mapper.Map<Comment>(newComment);

			var existingComment = await repository
				.GetAll().
				FirstAsync(entity => entity.Guid == Guid.Parse(newComment.Guid));

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
