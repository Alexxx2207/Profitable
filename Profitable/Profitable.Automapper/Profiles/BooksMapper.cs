namespace Profitable.Common.Automapper.Profiles
{
	using AutoMapper;
	using Profitable.Models.EntityModels;
	using Profitable.Models.ResponseModels.Books;

	public class BooksMapper : Profile
	{
		public BooksMapper()
		{
			CreateMap<Book, BookResponseModel>();
		}
	}
}
