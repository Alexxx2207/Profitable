namespace Profitable.Common.Automapper
{
    using AutoMapper;
    using Profitable.Common.Models.ScrapersModels;
    using Profitable.Models.EntityModels;

    public class COTMapper : Profile
    {
        public COTMapper()
        {
            CreateMap<ScrapeBigMoneyPositionsModel, COTReport>();
        }
    }
}
