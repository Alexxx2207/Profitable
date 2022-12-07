using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.ResponseModels.Books;
using Profitable.Services.Books.Contracts;

namespace Profitable.Services.Books
{
	public class BooksService : IBooksService
	{
		private readonly IMapper mapper;
		private readonly IRepository<Book> booksRepository;

		public BooksService(
			IMapper mapper,
			IRepository<Book> booksRepository)
		{
			this.mapper = mapper;
			this.booksRepository = booksRepository;
		}

		public async Task<List<BookResponseModel>> GetAllBooks(int page, int pageCount)
		{
			var books = await booksRepository
				.GetAllAsNoTracking()
				.OrderBy(book => book.Title)
				.Skip(page * pageCount)
				.Take(pageCount)
				.ToListAsync();

			return mapper.Map<List<BookResponseModel>>(books);
		}
	}
}
