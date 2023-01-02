namespace Profitable.Web.Hubs.Contracts
{
	using Profitable.Models.ResponseModels.Organizations;

	public interface IChatClient
	{
		Task ReceiveMessage(OrganizationMessageResponseModel message);
	}
}
