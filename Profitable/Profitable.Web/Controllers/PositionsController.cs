using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profitable.Common.Enums;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Positions;
using Profitable.Models.ResponseModels.Positions;
using Profitable.Services.Positions.Contracts;
using Profitable.Services.Users.Contracts;
using Profitable.Web.Controllers.BaseApiControllers;
using System.Security.Claims;

namespace Profitable.Web.Controllers
{
    public class PositionsController : BaseApiController
    {
        private readonly IPositionsRecordsService positionsRecordsService;
        private readonly IPositionsService positionsService;
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;

        public PositionsController(
            IPositionsRecordsService positionsRecordsService,
            IPositionsService positionsService,
            IUserService userService,
            UserManager<ApplicationUser> userManager)
        {
            this.positionsRecordsService = positionsRecordsService;
            this.positionsService = positionsService;
            this.userService = userService;
            this.userManager = userManager;
        }

        [HttpPost("records/by-user")]
        public async Task<IActionResult> GetAllPositionsRecordsByUser(
            [FromBody] GetUserPositionsRecordsRequestModel query)
        {
            var userGuid = (await userService.GetUserDetailsAsync(query.UserEmail)).Guid;

            Enum.TryParse(query.OrderPositionsRecordBy, out OrderPositionsRecordBy orderBy);

            var records = await positionsRecordsService.GetUserPositionsRecordsAsync(
                Guid.Parse(userGuid),
                query.Page,
                query.PageCount,
                orderBy);


            return Ok(records);
        }

        [HttpGet("records/order-options")]
        public IActionResult GetAllPositionsRecordsOrderByTypes()
        {
            return Ok(new OrderPositionsRecordsByTypes
            {
                Types = positionsRecordsService.GetPositionsRecordsOrderTypes()
            });
        }

        [Authorize]
        [HttpPost("records/create")]
        public async Task<IActionResult> CreatePositionsRecord(
            [FromBody] AddPositionsRecordRequestModel model)
        {
            var userGuid = (await userService.GetUserDetailsAsync(model.UserEmail)).Guid;

            var result = await positionsRecordsService.AddPositionsRecordList(
                Guid.Parse(userGuid),
                model.RecordName,
                model.InstrumentGroup);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Error);
            }
        }

        [Authorize]
        [HttpPatch("records/change/{recordGuid}")]
        public async Task<IActionResult> ChangePositionsRecord(
            [FromRoute] string recordGuid,
            [FromBody] ChangePositionsRecordRequestModel model)
        {

            var result = await positionsRecordsService.ChangeNamePositionsRecordList(
                Guid.Parse(recordGuid),
                model.RecordName);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Error);
            }
        }

        [Authorize]
        [HttpDelete("records/delete/{recordGuid}")]
        public async Task<IActionResult> DeletePositionsRecord([FromRoute] string recordGuid)
        {

            var result = await positionsRecordsService.DeletePositionsRecordList(
                Guid.Parse(recordGuid));

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Error);
            }
        }

        [HttpGet("records/{recordId}/positions")]
        public async Task<IActionResult> GetAllPositionsInARecord(
            [FromRoute] string recordId,
            [FromQuery] string afterDate)
        {
            if (afterDate != null)
            {
                var positions = await positionsService.GetFuturesPositions(
                    Guid.Parse(recordId),
                    DateTime.Parse(afterDate));
                return Ok(positions);

            }

            return BadRequest();
        }

        [HttpGet("records/{recordId}/positions/{positionGuid}")]
        public async Task<IActionResult> GetParticularPosition(
            [FromRoute] string recordId,
            [FromRoute] string positionGuid)
        {
            var positions =
                await positionsService.GetFuturesPositionByGuid(Guid.Parse(positionGuid));

            return Ok(positions);
        }

        [Authorize]
        [HttpPost("records/{recordId}/positions")]
        public async Task<IActionResult> CreatePosition(
            [FromRoute] string recordId,
            [FromBody] AddFuturesPositionRequestModel model)
        {
            var result =
                await positionsService.AddFuturesPositions(Guid.Parse(recordId), model);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Error);
            }
        }

        [Authorize]
        [HttpPatch("records/{recordGuid}/positions/{positionGuid}/change")]
        public async Task<IActionResult> ChangePositionByGuid(
           [FromRoute] string recordGuid,
           [FromRoute] string positionGuid,
           [FromBody] ChangeFuturesPositionRequestModel model)
        {
            var requesterGuid =
                (
                await userManager.FindByEmailAsync(this.User.FindFirstValue(ClaimTypes.Email))
                ).Id;


            var result = await positionsService.ChangeFuturesPosition(
                Guid.Parse(recordGuid),
                Guid.Parse(positionGuid),
                requesterGuid,
                model);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Error);
            }
        }

        [Authorize]
        [HttpDelete("records/{recordId}/positions/{positionGuid}/delete")]
        public async Task<IActionResult> DeletePosition(
            [FromRoute] string recordId,
            [FromRoute] string positionGuid)
        {
            var requesterGuid =
                (
                await userManager.FindByEmailAsync(this.User.FindFirstValue(ClaimTypes.Email))
                ).Id;

            var result = await positionsService.DeleteFuturesPositions(
                Guid.Parse(recordId),
                Guid.Parse(positionGuid),
                requesterGuid);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Error);
            }
        }
    }
}
