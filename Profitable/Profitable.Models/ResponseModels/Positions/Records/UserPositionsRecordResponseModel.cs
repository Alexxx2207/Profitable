namespace Profitable.Models.ResponseModels.Positions.Records
{
    public class UserPositionsRecordResponseModel
    {
        public string Guid { get; set; }

        public string Name { get; set; }

        public string LastUpdated { get; set; }

        public int PositionsCount { get; set; }

        public string InstrumentGroup { get; set; }
    }

}
