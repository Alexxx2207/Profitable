namespace Profitable.Models.ResponseModels.Positions.Records
{
    public class UserPositionsRecordResponseModel
    {
        public string Guid { get; set; }

        public string Name { get; set; }

        public DateTime LastUpdated { get; set; }

        public int PositionsCount { get; set; }

        public string InstrumentGroup { get; set; }
    }

}
