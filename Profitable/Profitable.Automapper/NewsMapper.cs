namespace Profitable.Common.Automapper
{
	using AutoMapper;
	using Profitable.Models.RequestModels.News;
	using Profitable.Models.ResponseModels.News;

	public class NewsMapper : Profile
	{
		public NewsMapper()
		{
			CreateMap<NewsArticleRequestModel, NewsArticleResponseModel>();

		}
	}
}
