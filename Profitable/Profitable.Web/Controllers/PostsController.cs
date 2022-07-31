using Microsoft.AspNetCore.Mvc;
using Profitable.Models.RequestModels.Posts;
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

        [Route("pages")]
        [HttpPost]
        public async Task<IActionResult> GetByPageAsync([FromBody] GetPostsRequestModel getPostsRequestModel)
        {
            try
            {
                var posts = await postService.GetPostsByPageAsync(getPostsRequestModel.Page, getPostsRequestModel.PostsCount);

                return Ok(posts);
            }
            catch (Exception err)
            {

                return BadRequest(err);
            }

        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var post = await postService.GetPostByGuidAsync(Guid.Parse(id));

            return Ok(post);
        }
    }
}
