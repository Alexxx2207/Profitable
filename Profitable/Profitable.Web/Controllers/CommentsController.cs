using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Comments;
using Profitable.Models.ResponseModels.Comments;
using Profitable.Services.Comments.Contracts;
using Profitable.Web.Controllers.BaseApiControllers;
using System.Security.Claims;

namespace Profitable.Web.Controllers
{
	public class CommentsController : BaseApiController
	{
		private readonly ICommentService commentService;
		private readonly UserManager<ApplicationUser> userManager;

		public CommentsController(ICommentService commentService, UserManager<ApplicationUser> userManager)
		{
			this.commentService = commentService;
			this.userManager = userManager;
		}

		[HttpGet("{postGuid}/count")]
		public async Task<IActionResult> GetCountByPost(Guid postGuid)
		{
			var commentsCount = await commentService.GetCommentsCountByPostAsync(postGuid);

			return Ok(commentsCount);
		}

		[HttpGet("{postGuid}/all/{page}/{pageCount}")]
		public async Task<IActionResult> GetAllByPost(Guid postGuid, string page, string pageCount)
		{

			var comments = await commentService.GetCommentsByPostAsync(postGuid, int.Parse(page), int.Parse(pageCount));

			return Ok(comments);
		}

		[Authorize]
		[HttpPost("{postGuid}/add")]
		public async Task<IActionResult> Add(Guid postGuid, AddCommentRequestModel postRequestModel)
		{
			var user = await userManager.FindByEmailAsync(this.User.FindFirstValue(ClaimTypes.Email));

			var result = await commentService.AddCommentAsync(new Comment
			{
				PostId = postGuid,
				Content = postRequestModel.Content,
				AuthorId = user.Id,
				PostedOn = DateTime.UtcNow,
			});

			if (result.Succeeded)
			{
				var response = new CommentResponseModel
				{
					AuthorEmail = user.Email,
					AuthorName = $"{user.FirstName} {user.LastName}",
					Content = postRequestModel.Content,
				};

				return Ok(response);
			}
			else
			{
				return BadRequest();
			}
		}
	}
}
