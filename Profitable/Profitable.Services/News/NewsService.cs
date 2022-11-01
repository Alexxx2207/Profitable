using AngleSharp;
using AutoMapper;
using Profitable.Models.RequestModels.News;
using Profitable.Models.ResponseModels.News;
using Profitable.Services.News.Contract;

namespace Profitable.Services.News
{
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
			var config = Configuration.Default.WithDefaultLoader();

			var context = BrowsingContext.New(config);

			var document = await context.OpenAsync(articleOverview.Link);

			var cellSelector = "section#leftColumn";

			var cell = document.QuerySelector(cellSelector);

			var articleText = string.Join("\\n", cell.QuerySelectorAll("div.articlePage>p")
								.Select(el => el.TextContent));

			var articleImage = cell.QuerySelector("div.articlePage>div#imgCarousel>img")?
							.GetAttribute("src");

			var article = mapper.Map<NewsArticleResponseModel>(articleOverview, opt =>
                opt.AfterMap((src, dest) => 
				{
					dest.Image = articleImage;
                    dest.ArticleText = articleText;
                }));

			return article;
		}

		public async Task<List<NewsOverviewResponseModel>> GetNewsOverviewFromInvestingCom(
			string address,
			List<NewsOverviewResponseModel> news)
		{
			var config = Configuration.Default.WithDefaultLoader();

			var context = BrowsingContext.New(config);

			var document = await context.OpenAsync(address);

			var cellSelector = "section#leftColumn div.largeTitle article.js-article-item";

			var cells = document.QuerySelectorAll(cellSelector);

			var images = cells
						.Select(m => m.QuerySelector("a.img img")?
									.GetAttribute("data-src"))
									.ToList();
			var titles = cells
						.Select(m => m.QuerySelector("div.textDiv a")?.TextContent)
						.ToList();

			var senders = cells
						.Select(m =>
							m.QuerySelector("div.textDiv span.articleDetails span:nth-child(1)")?.TextContent)
						.ToList();

			var timesPosted = cells
						.Select(m =>
							m.QuerySelector("div.textDiv span.articleDetails span.date")?.TextContent)
						.ToList();

			var textOverviews = cells
						.Select(m => m.QuerySelector("div.textDiv p")?.TextContent)
						.ToList();

			var links = cells
						.Select(m => m.QuerySelector("div.textDiv a")?
									.GetAttribute("href"))
									.ToList();

			for (int i = 0; i < titles.Count(); i++)
			{
				news.Add(new NewsOverviewResponseModel
				{
					Image = images[i]?.Trim(),
					Title = titles[i]?.Trim(),
					Sender = senders[i]?.Trim(),
					PostedAgo = timesPosted[i]?.Split("-")[1].Trim(),
					ArticleOverview = textOverviews[i]?.Trim(),
					Link = "https://www.investing.com" + links[i]?.Trim(),
				});
			}

			return news;
		}
	}
}
