namespace Profitable.Services.Organizations.Contracts
{
	using Profitable.Common.Models;
	using Profitable.Models.RequestModels.Organizations;

	public interface IOrganizationsService
	{

		Task<Result> AddMembersToOrganization(AddMembersRequestModel model);

		Task<Result> AddOrganization(AddOrganizationRequestModel model);

		Task<Result> UpdateOrganizationGeneralSettings(UpdateOrganizationRequestModel model);

		Task<Result> DeleteOrganization(Guid organization);
	}
}
