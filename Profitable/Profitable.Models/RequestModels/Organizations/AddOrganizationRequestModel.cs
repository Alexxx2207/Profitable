namespace Profitable.Models.RequestModels.Organizations
{
	public class AddOrganizationRequestModel
	{
		public string Name { get; set; }

		public Guid OwnerId { get; set; }
	}
}
