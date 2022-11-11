namespace Profitable.Models.RequestModels.Positions.Records
{
	public class GetUserPositionsRecordsRequestModel
	{
		public string UserEmail { get; set; }

		public int Page { get; set; }

		public int PageCount { get; set; }

		public string OrderPositionsRecordBy { get; set; }
	}
}
