namespace Profitable.Models.EntityModels
{
    using Profitable.Models.EntityModels.EntityBaseClass;

    public class COTReportedInstrument : EntityBase
    {
        public COTReportedInstrument()
        {
            COTReports = new HashSet<COTReport>();
        }

        public string InstrumentName { get; set; }

        public ICollection<COTReport> COTReports { get; set; }
    }
}
