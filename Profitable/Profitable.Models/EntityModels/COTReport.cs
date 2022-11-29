namespace Profitable.Models.EntityModels
{
    using Profitable.Models.EntityModels.EntityBaseClass;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class COTReport : EntityBase
    {
        [Required]
        public DateTime DatePublished { get; set; }

        public long AssetManagersLong { get; set; }

        public long AssetManagersShort { get; set; }

        public long LeveragedFundsLong { get; set; }

        public long LeveragedFundsShort { get; set; }

        public long AssetManagersLongChange { get; set; }

        public long AssetManagersShortChange { get; set; }

        public long LeveragedFundsLongChange { get; set; }

        public long LeveragedFundsShortChange { get; set; }


        [ForeignKey("COTReportedInstrument")]
        public Guid COTReportedInstrumentId { get; set; }
        public COTReportedInstrument COTReportedInstrument { get; set; }
    }
}
