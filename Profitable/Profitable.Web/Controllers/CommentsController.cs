namespace Profitable.Web.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Profitable.Models.EntityModels;
	using Profitable.Models.RequestModels.Comments;
	using Profitable.Services.Comments.Contracts;
	using Profitable.Web.Controllers.BaseApiControllers;

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
			var result = await commentService.AddCommentAsync(postGuid, postRequestModel, this.UserId);

			if (result.Succeeded)
			{
				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}

		[Authorize]
		[HttpPatch("{commentGuid}/update")]
		public async Task<IActionResult> Update(
			[FromRoute] Guid commentGuid,
			[FromBody] UpdateCommentRequestModel newComment)
		{
			var result = await commentService.UpdateCommentAsync(commentGuid, newComment, this.UserId);

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
			var result = await commentService.DeleteCommentAsync(commentGuid, this.UserId);

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
