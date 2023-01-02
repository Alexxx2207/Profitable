namespace Profitable.Models.RequestModels.Organizations
{
	public class GetOrganizationMessagesRequestModel
	{
		public Guid RequesterId { get; set; }

		public Guid OrganizationId { get; set; }

		public int Page { get; set; }

		public int PageCount { get; set; }
	}
}
