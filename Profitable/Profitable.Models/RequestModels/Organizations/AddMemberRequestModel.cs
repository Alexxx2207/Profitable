namespace Profitable.Models.RequestModels.Organizations
{
	public class AddMemberRequestModel
	{
		public Guid RequesterId { get; set; }

		public Guid OrganizationId { get; set; }

		public Guid MemberId { get; set; }
	}
}
