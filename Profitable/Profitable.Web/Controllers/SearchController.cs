using Microsoft.AspNetCore.Mvc;
using Profitable.Services.Search.Contracts;
using Profitable.Web.Controllers.BaseApiControllers;

namespace Profitable.Web.Controllers
{
	public class SearchController : BaseApiController
	{
		private readonly IUserSearchService userSearch;
		private readonly IPostSearchService postSearch;

		public SearchController(
			IUserSearchService userSearch,
			IPostSearchService postSearch)
		{
			this.userSearch = userSearch;
			this.postSearch = postSearch;
		}

		[HttpGet("users/{searchTerm}")]
		public async Task<IActionResult> Users([FromRoute] string searchTerm)
		{
			var usersFound = await userSearch.GetMatchingUsers(searchTerm);

			return Ok(usersFound);
		}

		[HttpGet("posts/{searchTerm}")]
		public async Task<IActionResult> Posts([FromRoute] string searchTerm)
		{
			var postsFound = await postSearch.GetMatchingPosts(searchTerm);

			return Ok(postsFound);
		}
	}
}
