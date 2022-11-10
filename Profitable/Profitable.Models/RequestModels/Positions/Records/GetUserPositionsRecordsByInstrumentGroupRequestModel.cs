namespace Profitable.Models.RequestModels.Positions.Records
{
	public class GetUserPositionsRecordsByInstrumentGroupRequestModel
	{
		public string UserEmail { get; set; }

		public int Page { get; set; }

		public int PageCount { get; set; }

		public string InstrumentGroup { get; set; }
	}
}
