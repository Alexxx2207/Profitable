namespace Profitable.Services.Search.Contracts
{
	using Profitable.Models.ResponseModels.Books;

	public interface IBookSearchService
	{
		Task<List<BookResponseModel>> GetMatchingBooks(string searchTerm, int page, int pageCount);
	}
}
