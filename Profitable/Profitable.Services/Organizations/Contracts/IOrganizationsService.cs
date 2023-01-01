namespace Profitable.Services.Organizations.Contracts
{
	using Profitable.Common.Models;
	using Profitable.Models.RequestModels.Organizations;
	using Profitable.Models.ResponseModels.Organizations;

	public interface IOrganizationsService
	{
		Task<OrganizationResponseModel> GetOrganization(Guid organizationId);

		Task<Result> AddOrganization(AddOrganizationRequestModel model);

		Task<Result> AddMessageInOrganization(AddMessageRequestModel model);

		Task<Result> UpdateOrganizationGeneralSettings(
			UpdateOrganizationRequestModel model);

		Task<Result> DeleteOrganization(DeleteOrganizationRequestModel model);

		Task<List<OrganizationMessageResponseModel>> GetOrganizationMessages(
			GetOrganizationMessagesRequestModel model);
	}
}
