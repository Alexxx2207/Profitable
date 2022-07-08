using Microsoft.AspNetCore.Mvc;
using Profitable.GlobalConstants;
using Profitable.Services.Posts.Contracts;

namespace Profitable.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }

        public async Task<IActionResult> GetRecentPosts()
        {
            var posts = await postService.GetRecentPosts(GlobalServicesConstants.RecentPostsCount);

            return Ok(posts);
        } 
    }
}
