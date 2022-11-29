namespace Profitable.Models.ResponseModels.COT
{
	public class COTReportResponseModel
	{
		public string DatePublished { get; set; }

		public string AssetManagersLong { get; set; }

		public string AssetManagersShort { get; set; }

		public string LeveragedFundsLong { get; set; }

		public string LeveragedFundsShort { get; set; }

		public string AssetManagersLongChange { get; set; }

		public string AssetManagersShortChange { get; set; }

		public string LeveragedFundsLongChange { get; set; }

		public string LeveragedFundsShortChange { get; set; }
	}
}
