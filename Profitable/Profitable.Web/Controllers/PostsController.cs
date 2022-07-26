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

        [Route("pages/{page}")]
        [HttpGet]
        public async Task<IActionResult> GetPostsByPage([FromRoute] int page)
        {
            var posts = await postService.GetPosts(page);

            return Ok(posts);
        } 

        [Route("{postId}")]
        [HttpGet]
        public async Task<IActionResult> GetPostById([FromRoute] string postId)
        {
            var post = await postService.GetPost(postId);

            return Ok(post);
        } 
    }
}
