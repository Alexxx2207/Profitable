using Microsoft.AspNetCore.Mvc;
using Profitable.Models.RequestModels.News;
using Profitable.Models.ResponseModels.News;
using Profitable.Services.News.Contract;
using Profitable.Web.Controllers.BaseApiControllers;

namespace Profitable.Web.Controllers
{
	public class NewsController : BaseApiController
	{
		private readonly INewsService newsService;

		public NewsController(INewsService newsService)
		{
			this.newsService = newsService;
		}

		[HttpGet("allNewsOverview")]
		public async Task<IActionResult> GetAllNewsOverviews()
		{
			var news = new List<NewsOverviewResponseModel>();

			await newsService.GetNewsOverviewFromInvestingCom(
				"https://www.investing.com/news/economy",
				news);
			await newsService.GetNewsOverviewFromInvestingCom(
				"https://www.investing.com/news/commodities-news",
				news);

			return Ok(news);
		}

		[HttpPost("newsArticle")]
		public async Task<IActionResult> GetNewsArticle(
			NewsArticleRequestModel newsArticleRequestModel)
		{
			var article =
					await newsService.GetNewsArticlesFromInvestingCom(newsArticleRequestModel);

			return Ok(article);
		}
	}
}
