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

        public OrganizationsController(
            IOrganizationsService organizationsService)
        {
            this.organizationsService = organizationsService;
        }

        [HttpPost("add-members")]
        [Authorize]
        public async Task<IActionResult> AddMembers(
            [FromBody] AddMembersRequestModel addMembersRequestModel)
        {
            var result = await organizationsService.AddMembersToOrganization(addMembersRequestModel);

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

            addRequestModel.OwnerId = UserId;

            var result = await organizationsService.AddOrganization(addRequestModel);

            if(result.Succeeded) 
            {
                return Ok();
            }

            return BadRequest(result.Error);
        }
    }
}
