namespace Profitable.Models.RequestModels.Positions.Records
{
    public class AddPositionsRecordRequestModel
    {
        public string UserEmail { get; set; }

        public string RecordName { get; set; }

        public string InstrumentGroup { get; set; }
    }
}
