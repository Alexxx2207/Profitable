namespace Profitable.Services.News
{
	using AngleSharp;
	using AutoMapper;
	using Profitable.Common.Scrapers;
	using Profitable.Models.RequestModels.News;
	using Profitable.Models.ResponseModels.News;
	using Profitable.Services.News.Contract;

	public class NewsService : INewsService
	{
		private readonly IMapper mapper;

		public NewsService(IMapper mapper)
		{
			this.mapper = mapper;
		}

		public async Task<NewsArticleResponseModel> GetNewsArticlesFromInvestingCom(
			NewsArticleRequestModel articleOverview)
		{
			return await InvestingComScraper.ScrapeNewsArticle(articleOverview, mapper);
        }

		public async Task<List<NewsOverviewResponseModel>> GetNewsOverviewFromInvestingCom(
			string address,
			List<NewsOverviewResponseModel> news)
		{
			return await InvestingComScraper.ScrapeNewsOverviews(address, news);
        }
    }
}
