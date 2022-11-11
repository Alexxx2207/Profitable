namespace Profitable.Web.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Profitable.Models.EntityModels;
	using Profitable.Models.RequestModels.Posts;
	using Profitable.Services.Comments.Contracts;
	using Profitable.Services.Posts.Contracts;
	using Profitable.Web.Controllers.BaseApiControllers;

	public class PostsController : BaseApiController
	{
		private readonly IPostService postService;
		private readonly ICommentService commentService;
		private readonly UserManager<ApplicationUser> userManager;

		public PostsController(IPostService postService,
			ICommentService commentService,
			UserManager<ApplicationUser> userManager)
		{
			this.postService = postService;
			this.commentService = commentService;
			this.userManager = userManager;
		}

		[HttpGet("feed/{page}/{pageCount}")]
		public async Task<IActionResult> GetPostFeedAsync(
			[FromRoute] int page,
			[FromRoute] int pageCount,
			[FromQuery] string? loggedInUserEmail)
		{
			try
			{
				var posts =
						await postService.GetPostsByPageAsync(
							page,
							pageCount,
							loggedInUserEmail);

				foreach (var post in posts)
				{
					var commentsCount =
							await commentService.GetCommentsCountByPostAsync(
								Guid.Parse(post.Guid));

					post.CommentsCount = commentsCount;
				}

				return Ok(posts);
			}
			catch (Exception err)
			{
				return BadRequest(err);
			}
		}

		[HttpGet("byuser/all/{page}/{pageCount}")]
		public async Task<IActionResult> GetPostsByUserAsync(
			[FromRoute] string page,
			[FromRoute] string pageCount,
			[FromQuery] string userEmail)
		{
			try
			{
				var user = await userManager.FindByEmailAsync(userEmail);

				var posts =
					await postService.GetPostsByUserAsync(
						user.Id,
						int.Parse(page),
						int.Parse(pageCount));

				foreach (var post in posts)
				{
					var commentsCount =
						await commentService.GetCommentsCountByPostAsync(
							Guid.Parse(post.Guid));

					post.CommentsCount = commentsCount;
				}

				return Ok(posts);
			}
			catch (Exception err)
			{
				return BadRequest(err.Message);
			}
		}

		[Authorize]
		[HttpPost("create")]
		public async Task<IActionResult> CreateAsync(
			[FromBody] AddPostRequestModel addPostRequestModel)
		{
			var result =
						await postService.AddPostAsync(this.UserId, addPostRequestModel);

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
		[HttpPost("{postGuid}/likes/manage")]
		public async Task<IActionResult> ManageLikeAsync(
			[FromRoute] string postGuid)
		{
			var likesCount =
					await postService.ManagePostLikeAsync(this.UserId, Guid.Parse(postGuid));

			return Ok(likesCount);
		}

		[Authorize]
		[HttpPatch("{postGuid}/edit")]
		public async Task<IActionResult> EditAsync(
			[FromRoute] string postGuid,
			[FromBody] UpdatePostRequestModel editPostRequestModel)
		{
			var result =
						await postService.UpdatePostAsync(Guid.Parse(postGuid), editPostRequestModel, this.UserId);

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
		[HttpDelete("{postGuid}/delete")]
		public async Task<IActionResult> DeleteAsync([FromRoute] Guid postGuid)
		{
			var result = await postService.DeletePostAsync(postGuid, this.UserId);

			if (result.Succeeded)
			{
				return Ok();
			}
			else
			{
				return BadRequest(result.Error);
			}

		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetByIdAsync(
			[FromRoute] string id,
			[FromQuery] string? loggedInUserEmail)
		{
			try
			{
				var post = await postService.GetPostByGuidAsync(Guid.Parse(id), loggedInUserEmail);

				return Ok(post);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

		}
	}
}
