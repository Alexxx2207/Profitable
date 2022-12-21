namespace Profitable.Models.RequestModels.Organizations
{
	public class UpdateOrganizationRequestModel
	{
		public Guid RequesterId { get; set; }

		public Guid OrganizationIdToUpdate { get; set; }

		public string Name { get; set; }
	}
}
