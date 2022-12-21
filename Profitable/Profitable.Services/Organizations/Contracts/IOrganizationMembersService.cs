namespace Profitable.Services.Organizations.Contracts
{
	using Profitable.Common.Models;
	using Profitable.Models.RequestModels.Organizations;

	public interface IOrganizationMembersService
	{
		Task<Result> AddMembersToOrganization(AddMembersRequestModel model);

		Task<Result> RemoveMemberFromOrganization(RemoveMemberRequestModel model);

		Task<Result> ChangeMemberRoleToOrganization(ChangeMemberRoleRequestModel model);
	}
}
