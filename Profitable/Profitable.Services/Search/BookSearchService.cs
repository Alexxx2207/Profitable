namespace Profitable.Services.Search
{
	using AutoMapper;
	using Microsoft.EntityFrameworkCore;
	using Profitable.Data.Repository.Contract;
	using Profitable.Models.EntityModels;
	using Profitable.Models.ResponseModels.Books;
	using Profitable.Services.Search.Contracts;

	public class BookSearchService : IBookSearchService
	{
		private readonly IMapper mapper;
		private readonly IRepository<Book> bookRepository;

		public BookSearchService(
			IMapper mapper,
			IRepository<Book> bookRepository)
		{
			this.mapper = mapper;
			this.bookRepository = bookRepository;
		}

		public async Task<List<BookResponseModel>> GetMatchingBooks(
			string searchTerm,
			int page,
			int pageCount)
		{
			searchTerm = searchTerm.ToLower();

			var books = await bookRepository
				.GetAllAsNoTracking()
				.OrderBy(b => b.Title.ToLower())
				.Where(b =>
					!b.IsDeleted &&
					(b.Title.ToLower().Contains(searchTerm)
					||
					b.Authors.ToLower().Contains(searchTerm)))
				.Skip(page * pageCount)
				.Take(pageCount)
				.ToListAsync();

			return mapper.Map<List<BookResponseModel>>(books);
		}
	}
}
