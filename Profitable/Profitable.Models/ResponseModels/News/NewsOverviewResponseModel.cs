﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.ResponseModels.News
{
	public class NewsOverviewResponseModel
	{
		public string Image { get; set; }

		public string Title { get; set; }

		public string Sender { get; set; }

		public string PostedAgo { get; set; }

		public string ArticleOverview { get; set; }

		public string Link { get; set; }
	}
}
