namespace Profitable.Web.Hubs
{
	using Microsoft.AspNetCore.SignalR;
	using Profitable.Models.ResponseModels.Organizations;
	using Profitable.Web.Hubs.Contracts;

	public class ChatHub : Hub<IChatClient>
	{
		public async Task SendMessage(OrganizationMessageResponseModel message)
		{
			await Clients.All.ReceiveMessage(message);
		}
	}
}
