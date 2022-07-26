using Microsoft.AspNetCore.Mvc;
using Profitable.Services.Posts.Contracts;
using Profitable.Web.Controllers.Contracts;

namespace Profitable.Web.Controllers
{
    public class PostsController : BaseApiController
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }

        [Route("pages/{page}")]
        [HttpGet]
        public async Task<IActionResult> GetPostsByPageAsync([FromRoute] int page)
        {
            var posts = await postService.GetPostsAsync(page);

            return Ok(posts);
        }

        [Route("{postId}")]
        [HttpGet]
        public async Task<IActionResult> GetPostByIdAsync([FromRoute] string postId)
        {
            var post = await postService.GetPostAsync(postId);

            return Ok(post);
        }
    }
}
