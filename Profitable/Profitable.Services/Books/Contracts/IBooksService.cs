using Profitable.Models.ResponseModels.Books;

namespace Profitable.Services.Books.Contracts
{
	public interface IBooksService
	{
		Task<List<BookResponseModel>> GetAllBooks(int page, int pageCount);
	}
}
