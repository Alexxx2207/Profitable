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

        [Route("{page}")]
        [HttpGet]
        public async Task<IActionResult> GetPostsByPage([FromRoute] int page)
        {
            var posts = await postService.GetPosts(page);

            return Ok(posts);
        } 
    }
}
