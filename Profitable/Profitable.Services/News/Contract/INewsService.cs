namespace Profitable.Services.News.Contract
{
	using Profitable.Models.RequestModels.News;
	using Profitable.Models.ResponseModels.News;

	public interface INewsService
	{
		Task<List<NewsOverviewResponseModel>> GetNewsOverviewFromInvestingCom(string address, List<NewsOverviewResponseModel> news);
		Task<NewsArticleResponseModel> GetNewsArticlesFromInvestingCom(NewsArticleRequestModel articleOverview);
	}
}
