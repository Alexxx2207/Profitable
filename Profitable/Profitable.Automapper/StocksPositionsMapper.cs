using AutoMapper;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Positions.Futures;
using Profitable.Models.RequestModels.Positions.Stocks;
using Profitable.Models.ResponseModels.Positions.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Common.Automapper
{
    public class StocksPositionsMapper : Profile
    {
        public StocksPositionsMapper()
        {
            CreateMap<AddStocksPositionRequestModel, TradePosition>();

            CreateMap<AddStocksPositionRequestModel, StocksPosition>();

            CreateMap<TradePosition, StocksPositionResponseModel>();

            CreateMap<StocksPosition, StocksPositionResponseModel>()
               .ForMember(
                   dest => dest.Guid,
                   opt => opt.Ignore());
        }
    }
}
