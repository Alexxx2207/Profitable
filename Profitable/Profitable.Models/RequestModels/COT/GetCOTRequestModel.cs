namespace Profitable.Models.RequestModels.COT
{
	public class GetCOTRequestModel
	{
		public Guid InstrumentGuid { get; set; }

		public string InstrumentName { get; set; }

		public DateTime FromDate { get; set; }
	}
}
