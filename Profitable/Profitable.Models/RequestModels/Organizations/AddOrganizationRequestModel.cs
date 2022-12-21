namespace Profitable.Models.RequestModels.Organizations
{
	public class AddOrganizationRequestModel
	{
		public Guid RequesterId { get; set; }

		public string Name { get; set; }
	}
}
