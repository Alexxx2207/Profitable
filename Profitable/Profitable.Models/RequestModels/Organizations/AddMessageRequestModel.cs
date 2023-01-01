namespace Profitable.Models.RequestModels.Organizations
{
	public class AddMessageRequestModel
	{
		public Guid SenderId { get; set; }

		public Guid OrganizationId { get; set; }

		public string Message { get; set; }
	}
}
