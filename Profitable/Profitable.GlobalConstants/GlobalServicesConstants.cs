namespace Profitable.Common.GlobalConstants
{
	public static class GlobalServicesConstants
	{

		public static readonly int PostsMaxCountInPage = 100;

		public static readonly short PostMaxLength = 2000;

		public static readonly short CommentMaxLength = 1000;

		public static readonly string UploadsFolderInProject = "Profitable.Web";

		public static readonly string RequesterNotOwnerMesssage = "Requester not owner of the entity";

		public static readonly char DirectorySeparatorChar = Path.DirectorySeparatorChar;

		public static readonly string UploadsFolderPath =
			Path.GetDirectoryName(Directory.GetCurrentDirectory())
			+ $"{DirectorySeparatorChar}{UploadsFolderInProject}{DirectorySeparatorChar}Uploads";

		public static string EntityDoesNotExist(string entity) => $"{entity} was not found!";

		public static readonly int WeeksOfCOTReportsForFirstSeed = 10;

		public static readonly Dictionary<string, string> CotReportSourcesLinks = new Dictionary<string, string>
		{
			{
				"S&P 500 Consolidated - CHICAGO MERCANTILE EXCHANGE",
				"https://www.tradingster.com/cot/futures/fin/13874"
			},
			{
				"NASDAQ-100 Consolidated - CHICAGO MERCANTILE EXCHANGE",
				"https://www.tradingster.com/cot/futures/fin/20974"
			}
		};
	}
}
