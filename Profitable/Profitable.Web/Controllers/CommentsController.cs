using Microsoft.AspNetCore.Mvc;
using Profitable.Models.ResponseModels.Comments;
using Profitable.Services.Comments.Contracts;
using Profitable.Web.Controllers.BaseApiControllers;

namespace Profitable.Web.Controllers
{
	public class CommentsController : BaseApiController
	{
		private readonly ICommentService commentService;

		public CommentsController(ICommentService commentService)
		{
			this.commentService = commentService;
		}

		[HttpGet("{postGuid}/count")]
		public async Task<int> GetCountByPost(Guid postGuid)
		{
			return await commentService.GetCommentsCountByPostAsync(postGuid);
		}

		[HttpGet("{postGuid}/all")]
		public async Task<List<CommentResponseModel>> GetAllByPost(Guid postGuid)
		{
			return await commentService.GetCommentsByPostAsync(postGuid);
		}
	}
}
