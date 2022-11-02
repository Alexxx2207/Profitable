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

		public CommentsController(
			ICommentService commentService,
			UserManager<ApplicationUser> userManager)
		{
			this.commentService = commentService;
			this.userManager = userManager;
		}

		[HttpGet("{postGuid}/count")]
		public async Task<IActionResult> GetCountByPost(Guid postGuid)
		{
			var commentsCount =
					await commentService.GetCommentsCountByPostAsync(postGuid);

			return Ok(commentsCount);
		}

		[HttpGet("bypost/{postGuid}/all/{page}/{pageCount}")]
		public async Task<IActionResult> GetAllByPost(
			Guid postGuid,
			string page,
			string pageCount)
		{

			var comments =
					await commentService.GetCommentsByPostAsync(
						postGuid,
						int.Parse(page),
						int.Parse(pageCount));

			return Ok(comments);
		}

		[HttpGet("byuser/all/{page}/{pageCount}")]
		public async Task<IActionResult> GetAllByUser(
			[FromRoute] string page,
            [FromRoute] string pageCount,
            [FromQuery] string userEmail)
		{
			var user = await userManager.FindByEmailAsync(userEmail);

			var comments =
				await commentService.GetCommentsByUserAsync(
					user.Id,
					int.Parse(page),
					int.Parse(pageCount));

			return Ok(comments);
		}

		[Authorize]
		[HttpPost("{postGuid}/add")]
		public async Task<IActionResult> Add(
			Guid postGuid,
			AddCommentRequestModel postRequestModel)
		{
			var user =
				await userManager.FindByEmailAsync(this.User.FindFirstValue(ClaimTypes.Email));

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

		[Authorize]
		[HttpPatch("{commentGuid}/update")]
		public async Task<IActionResult> Update(
			Guid commentGuid,
			UpdateCommentRequestModel newComment)
		{
			var result = await commentService.UpdateCommentAsync(commentGuid, newComment);

			if (result.Succeeded)
			{
				return Ok();
			}
			else
			{
				return BadRequest(result.Error);
			}
		}

		[Authorize]
		[HttpDelete("{commentGuid}/delete")]
		public async Task<IActionResult> Delete(Guid commentGuid)
		{
			var result = await commentService.DeleteCommentAsync(commentGuid);

			if (result.Succeeded)
			{
				return Ok();
			}
			else
			{
				return BadRequest(result.Error);
			}
		}
	}
}
