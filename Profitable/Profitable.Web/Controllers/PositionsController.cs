using Microsoft.AspNetCore.Mvc;
using Profitable.Common.Enums;
using Profitable.Models.RequestModels.Users;
using Profitable.Services.Positions.Contracts;
using Profitable.Services.Users.Contracts;
using Profitable.Web.Controllers.BaseApiControllers;

namespace Profitable.Web.Controllers
{
    public class PositionsController : BaseApiController
	{
        private readonly IPositionsRecordsService positionsRecordsService;
        private readonly IUserService userService;

        public PositionsController(
            IPositionsRecordsService positionsRecordsService,
            IUserService userService)
        {
            this.positionsRecordsService = positionsRecordsService;
            this.userService = userService;
        }

        [HttpPost("records/byuser")]
        public async Task<IActionResult> GetAllPositionsRecordsByUser(
            [FromBody] GetUserPositionsRecordsRequestModel query)
        {
            var userGuid = Guid.Parse((await userService.GetUserDetailsAsync(query.UserEmail)).Guid);

            var records = await positionsRecordsService.GetUserPositionsRecordsAsync(
				userGuid,
                query.Page,
                query.PageCount,
                (OrderPositionsRecordBy) query.OrderPositionsRecordBy);


            return Ok(records);
		}
    }
}
