using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profitable.Common.Enums;
using Profitable.Models.RequestModels.Positions;
using Profitable.Models.ResponseModels.Positions;
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

        [HttpPost("records/by-user")]
        public async Task<IActionResult> GetAllPositionsRecordsByUser(
            [FromBody] GetUserPositionsRecordsRequestModel query)
        {
            var userGuid = Guid.Parse((await userService.GetUserDetailsAsync(query.UserEmail)).Guid);

            Enum.TryParse(query.OrderPositionsRecordBy, out OrderPositionsRecordBy orderBy);

            var records = await positionsRecordsService.GetUserPositionsRecordsAsync(
                userGuid,
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
            var userGuid = Guid.Parse((await userService.GetUserDetailsAsync(model.UserEmail)).Guid);

            var records = await positionsRecordsService.AddPositionsRecordList(
                userGuid,
                model.RecordName,
                model.InstrumentGroup);

            if (records.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(records.Error);
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
    }
}
