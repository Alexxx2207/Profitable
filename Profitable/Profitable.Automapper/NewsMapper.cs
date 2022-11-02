using AutoMapper;
using Profitable.Models.RequestModels.News;
using Profitable.Models.ResponseModels.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Common.Automapper
{
    public class NewsMapper : Profile
    {
        public NewsMapper()
        {
            CreateMap<NewsArticleRequestModel, NewsArticleResponseModel>();

        }
    }
}
