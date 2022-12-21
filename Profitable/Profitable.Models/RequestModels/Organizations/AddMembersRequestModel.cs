namespace Profitable.Models.RequestModels.Organizations
{
	public class AddMembersRequestModel
	{
		public Guid RequesterId { get; set; }

		public Guid OrganizationId { get; set; }

		public List<Guid> Members { get; set; }
	}
}
