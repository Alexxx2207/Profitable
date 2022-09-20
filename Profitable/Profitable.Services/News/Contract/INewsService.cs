using Profitable.Models.RequestModels.News;
using Profitable.Models.ResponseModels.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Services.News.Contract
{
	public interface INewsService
	{
		Task<List<NewsOverviewResponseModel>> GetNewsOverviewFromInvestingCom(string address, List<NewsOverviewResponseModel> news);
		Task<NewsArticleResponseModel> GetNewsArticlesFromInvestingCom(NewsArticleRequestModel articleOverview);
	}
}
