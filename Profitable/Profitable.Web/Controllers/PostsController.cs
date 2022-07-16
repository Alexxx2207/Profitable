using Microsoft.AspNetCore.Mvc;
using Profitable.GlobalConstants;
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

        [Route("recent")]
        [HttpGet]
        public async Task<IActionResult> GetRecentPosts()
        {
            var posts = await postService.GetRecentPosts();

            return Ok(posts);
        } 
    }
}
