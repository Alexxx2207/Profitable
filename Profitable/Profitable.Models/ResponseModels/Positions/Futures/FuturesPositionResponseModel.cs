using Profitable.Common.Enums;

namespace Profitable.Models.ResponseModels.Positions.Futures
{
    public class FuturesPositionResponseModel
    {
        public string Guid { get; set; }

        public string PositionAddedOn { get; set; }

        public string ContractName { get; set; }

        public string Direction { get; set; }

        public string EntryPrice { get; set; }

        public string ExitPrice { get; set; }

        public string QuantitySize { get; set; }

        public string TickSize { get; set; }

        public string TickValue { get; set; }

        public string RealizedProfitAndLoss { get; set; }
    }
}
