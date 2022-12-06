namespace Profitable.Common.Automapper.Profiles
{
    using AutoMapper;
    using Profitable.Common.Models.ScrapersModels;
    using Profitable.Models.EntityModels;
    using Profitable.Models.ResponseModels.COT;

    public class COTMapper : Profile
    {
        public COTMapper()
        {
            CreateMap<ScrapeBigMoneyPositionsModel, COTReport>();
            CreateMap<COTReport, COTReportResponseModel>();
            CreateMap<COTReportedInstrument, COTReportedInstrumentResponseModel>();
        }
    }
}
