namespace Profitable.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Profitable.Models.RequestModels.Organizations;
    using Profitable.Models.RequestModels.Users;
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

        [HttpPost("add-members")]
        [Authorize]
        public async Task<IActionResult> AddMembers(
            [FromBody] AddMembersRequestModel addMembersRequestModel)
        {
            addMembersRequestModel.RequesterId = UserId;

            var result = await organizationMembersService
                .AddMembersToOrganization(addMembersRequestModel);

            if(result.Succeeded) 
            {
                return Ok();
            }

            return BadRequest(result.Error);
        }
        
        [HttpPost("change-member-role")]
        [Authorize]
        public async Task<IActionResult> ChangeMemberRole(
            [FromBody] ChangeMemberRoleRequestModel changeMemberRequestModel)
        {
            changeMemberRequestModel.RequesterId = UserId;

            var result = await organizationMembersService
                .ChangeMemberRoleToOrganization(changeMemberRequestModel);

            if(result.Succeeded) 
            {
                return Ok();
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

            if(result.Succeeded) 
            {
                return Ok();
            }

            return BadRequest(result.Error);
        }
        
        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> Add(
            [FromBody] AddOrganizationRequestModel addRequestModel)
        {

            addRequestModel.RequesterId = UserId;

            var result = await organizationsService.AddOrganization(addRequestModel);

            if(result.Succeeded) 
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

            if(result.Succeeded) 
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

            if(result.Succeeded) 
            {
                return Ok();
            }

            return BadRequest(result.Error);
        }
    }
}
