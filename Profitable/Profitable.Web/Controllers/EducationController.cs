namespace Profitable.Web.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Profitable.Services.Books.Contracts;
	using Profitable.Web.Controllers.BaseApiControllers;

	public class EducationController : BaseApiController
	{
		private readonly IBooksService booksService;

		public EducationController(IBooksService booksService)
		{
			this.booksService = booksService;
		}

		[HttpGet("books")]
		public async Task<IActionResult> GetBooks(
			[FromQuery] int page,
			[FromQuery] int pageCount)
		{
			var books = await booksService.GetAllBooks(page, pageCount);

			return Ok(books);
		}
	}
}
