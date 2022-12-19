namespace Profitable.Services.Organizations.Contracts
{
	using Profitable.Common.Models;
	using Profitable.Models.RequestModels.Organizations;

	public interface IMembersService
	{
		Task<Result> AddMembersToOrganization(AddMembersRequestModel addMembersRequestModel);

		Task<Result> RemoveMemberToOrganization(RemoveMemberRequestModel removeMemberRequestModel);

		Task<Result> ChangeMemberRoleToOrganization(ChangeMemberRoleRequestModel changeMemberRoleRequestModel);
	}
}
