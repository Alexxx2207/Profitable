namespace Profitable.Models.RequestModels.Journals
{
	public class UpdateJournalRequestModel
	{
		public Guid JournalId { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }
	}
}
