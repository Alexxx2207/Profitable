using Microsoft.AspNetCore.Mvc;
using Profitable.Services.Markets.Contract;
using Profitable.Web.Controllers.Contracts;

namespace Profitable.Web.Controllers
{
    public class MarketsController : BaseApiController
    {
        private readonly IMarketsService marketsService;

        public MarketsController(IMarketsService marketsService)
        {
            this.marketsService = marketsService;
        }

        [Route("{symbol}")]
        [HttpGet]
        public async Task<IActionResult> GetInstrumentAsync([FromRoute] string symbol)
        {
            var instrument = await this.marketsService.GetFinantialInstrumentBySymbolAsync(symbol);

            return Ok(instrument);
        }

        [HttpGet]
        public async Task<IActionResult> GetInstrumentsAsync()
        {
            var instrument = await this.marketsService.GetAllFinantialInstrumentsAsync();

            return Ok(instrument);
        }
    }
}
