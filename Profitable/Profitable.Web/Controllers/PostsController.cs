using Microsoft.AspNetCore.Mvc;
using Profitable.Services.Posts.Contracts;
using Profitable.Web.Controllers.BaseApiControllers;

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
        public async Task<IActionResult> GetByPageAsync([FromRoute] int page)
        {
            var posts = await postService.GetPostsByPageAsync(page);

            return Ok(posts);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var post = await postService.GetPostByGuidAsync(id);

            return Ok(post);
        }
    }
}
