using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Posts;
using Profitable.Services.Comments.Contracts;
using Profitable.Services.Posts.Contracts;
using Profitable.Web.Controllers.BaseApiControllers;
using System.Security.Claims;

namespace Profitable.Web.Controllers
{
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
        public async Task<IActionResult> GetPostFeedAsync(int page, int pageCount)
        {
            try
            {
                var posts = await postService.GetPostsByPageAsync(page, pageCount);

                foreach (var post in posts)
                {
                    var commentsCount = await commentService.GetCommentsCountByPostAsync(Guid.Parse(post.Guid));

                    post.CommentsCount = commentsCount;
                }

                return Ok(posts);
            }
            catch (Exception err)
            {

                return BadRequest(err);
            }
        }

        [Authorize]
        [HttpGet("byuser/all/{page}/{pageCount}")]
        public async Task<IActionResult> GetPostsByUserAsync(string page, string pageCount)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(this.User.FindFirstValue(ClaimTypes.Email));

                var posts = await postService.GetPostsByUserAsync(user.Id, int.Parse(page), int.Parse(pageCount));

                foreach (var post in posts)
                {
                    var commentsCount = await commentService.GetCommentsCountByPostAsync(Guid.Parse(post.Guid));

                    post.CommentsCount = commentsCount;
                }


                return Ok(posts);
            }
            catch (Exception err)
            {

                return BadRequest(err);
            }
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] AddPostRequestModel addPostRequestModel)
        {
            try
            {
                var author = await userManager.FindByEmailAsync(this.User.FindFirstValue(ClaimTypes.Email));

                var postToCreate = await postService.AddPostAsync(author, addPostRequestModel);

                return Ok(postToCreate);
            }
            catch (Exception err)
            {

                return BadRequest(err.Message);
            }

        }

        [Authorize]
        [HttpPost("{guid}/likes/manage")]
        public async Task<IActionResult> ManageLikeAsync([FromRoute] string guid)
        {
            var author = await userManager.FindByEmailAsync(this.User.FindFirstValue(ClaimTypes.Email));

            var likesCount = await postService.ManagePostLikeAsync(author, guid);

            return Ok(likesCount);
        }

        [Authorize]
        [HttpPatch("{guid}/edit")]
        public async Task<IActionResult> EditAsync([FromRoute] string guid, [FromBody] UpdatePostRequestModel editPostRequestModel)
        {
            try
            {
                var postToCreate = await postService.UpdatePostAsync(guid, editPostRequestModel);

                return Ok(postToCreate);
            }
            catch (Exception err)
            {

                return BadRequest(err.Message);
            }

        }

        [Authorize]
        [HttpDelete("{guid}/delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid guid)
        {
            var result = await postService.DeletePostAsync(guid);

            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }
            else
            {
                return BadRequest(result.Error);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var post = await postService.GetPostByGuidAsync(Guid.Parse(id));

            return Ok(post);
        }
    }
}
