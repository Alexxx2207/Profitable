﻿using Microsoft.AspNetCore.Mvc;
using Profitable.Services.Markets.Contract;

namespace Profitable.Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MarketsController : ControllerBase
    {
        private readonly IMarketsService marketsService;

        public MarketsController(IMarketsService marketsService)
        {
            this.marketsService = marketsService;
        }

        [Route("{symbol}")]
        [HttpGet]
        public async Task<IActionResult> GetInstrument([FromRoute] string symbol)
        {
            var instrument = await this.marketsService.GetFinantialInstrumentBySymbol(symbol);

            return Ok(instrument);
        }
    }
}