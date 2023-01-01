namespace Profitable.Web.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Profitable.Models.RequestModels.Organizations;
	using Profitable.Services.Organizations.Contracts;
	using Profitable.Web.Controllers.BaseApiControllers;

	public class OrganizationsController : BaseApiController
	{
		private readonly IOrganizationsService organizationsService;
		private readonly IOrganizationMembersService organizationMembersService;

		public OrganizationsController(
			IOrganizationsService organizationsService,
			IOrganizationMembersService organizationMembersService)
		{
			this.organizationsService = organizationsService;
			this.organizationMembersService = organizationMembersService;
		}

		[HttpPatch("add-member")]
		[Authorize]
		public async Task<IActionResult> AddMembers(
			[FromBody] AddMemberRequestModel addMembersRequestModel)
		{
			addMembersRequestModel.RequesterId = UserId;

			var result = await organizationMembersService
				.AddMemberToOrganization(addMembersRequestModel);

			if (result.Succeeded)
			{
				return Ok();
			}

			return BadRequest(result.Error);
		}

		[HttpPatch("change-member-role")]
		[Authorize]
		public async Task<IActionResult> ChangeMemberRole(
			[FromBody] ChangeMemberRoleRequestModel changeMemberRequestModel)
		{
			changeMemberRequestModel.RequesterId = UserId;

			var result = await organizationMembersService
				.ChangeMemberRoleToOrganization(changeMemberRequestModel);

			if (result.Succeeded)
			{
				return Ok(changeMemberRequestModel.RoleToAssign);
			}

			return BadRequest(result.Error);
		}

		[HttpPatch("remove-member")]
		[Authorize]
		public async Task<IActionResult> RemoveMember(
			[FromBody] RemoveMemberRequestModel removeMemberRequestModel)
		{
			removeMemberRequestModel.RequesterId = UserId;

			var result = await organizationMembersService
				.RemoveMemberFromOrganization(removeMemberRequestModel);

			if (result.Succeeded)
			{
				return Ok();
			}

			return BadRequest(result.Error);
		}

		[HttpPost("add-organization")]
		[Authorize]
		public async Task<IActionResult> Add(
			[FromBody] AddOrganizationRequestModel addRequestModel)
		{

			addRequestModel.RequesterId = UserId;

			var result = await organizationsService.AddOrganization(addRequestModel);

			if (result.Succeeded)
			{
				return Ok();
			}

			return BadRequest(result.Error);
		}

		[HttpPatch("update")]
		[Authorize]
		public async Task<IActionResult> Update(
			[FromBody] UpdateOrganizationRequestModel updateRequestModel)
		{
			updateRequestModel.RequesterId = UserId;

			var result = await organizationsService.UpdateOrganizationGeneralSettings(updateRequestModel);

			if (result.Succeeded)
			{
				return Ok();
			}

			return BadRequest(result.Error);
		}

		[HttpDelete("delete")]
		[Authorize]
		public async Task<IActionResult> Delete(
			[FromQuery] DeleteOrganizationRequestModel deleteRequestModel)
		{
			deleteRequestModel.RequesterId = UserId;

			var result = await organizationsService.DeleteOrganization(deleteRequestModel);

			if (result.Succeeded)
			{
				return Ok();
			}

			return BadRequest(result.Error);
		}

		[HttpGet("{organizationId}/get-organization")]
		public async Task<IActionResult> Get(
			[FromRoute] Guid organizationId)
		{
			var result = await organizationsService.GetOrganization(organizationId);

			return Ok(result);
		}

		[HttpGet("{organizationId}/get-messages")]
		[Authorize]
		public async Task<IActionResult> GetMessages(
			[FromRoute] Guid organizationId,
			[FromQuery] int page,
			[FromQuery] int pageCount)
		{
			try
			{
				var request = new GetOrganizationMessagesRequestModel()
				{
					RequesterId = UserId,
					OrganizationId = organizationId,
					Page = page,
					PageCount = pageCount
				};
				var result = await organizationsService
					.GetOrganizationMessages(request);

				result.Reverse();

				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost("{organizationId}/add-message")]
		[Authorize]
		public async Task<IActionResult> AddMessage(
			[FromBody] AddMessageRequestModel request)
		{
			try
			{
				request.SenderId = UserId;

				var result = await organizationsService.AddMessageInOrganization(request);

				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}
