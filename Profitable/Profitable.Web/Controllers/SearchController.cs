namespace Profitable.Web.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Profitable.Services.Search.Contracts;
	using Profitable.Web.Controllers.BaseApiControllers;

	public class SearchController : BaseApiController
	{
		private readonly IUserSearchService userSearch;
		private readonly IPostSearchService postSearch;
		private readonly IBookSearchService bookSearch;

		public SearchController(
			IUserSearchService userSearch,
			IPostSearchService postSearch,
			IBookSearchService bookSearch)
		{
			this.userSearch = userSearch;
			this.postSearch = postSearch;
			this.bookSearch = bookSearch;
		}

		[HttpGet("users/{searchTerm}")]
		public async Task<IActionResult> Users(
			[FromRoute] string searchTerm,
			[FromQuery] int page,
			[FromQuery] int pageCount)
		{
			var usersFound = await userSearch.GetMatchingUsers(searchTerm, page, pageCount);

			return Ok(usersFound);
		}

		[HttpGet("posts/{searchTerm}")]
		public async Task<IActionResult> Posts(
			[FromRoute] string searchTerm,
			[FromQuery] int page,
			[FromQuery] int pageCount)
		{
			var postsFound = await postSearch.GetMatchingPosts(searchTerm, page, pageCount);

			return Ok(postsFound);
		}

		[HttpGet("books/{searchTerm}")]
		public async Task<IActionResult> Books(
			[FromRoute] string searchTerm,
			[FromQuery] int page,
			[FromQuery] int pageCount)
		{
			var postsFound = await bookSearch.GetMatchingBooks(searchTerm, page, pageCount);

			return Ok(postsFound);
		}
	}
}
