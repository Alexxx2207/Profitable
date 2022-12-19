namespace Profitable.Models.RequestModels.Organizations
{
	public class UpdateOrganizationRequestModel
	{
		public Guid OrganizationIdToUodate { get; set; }

		public string Name { get; set; }
	}
}
