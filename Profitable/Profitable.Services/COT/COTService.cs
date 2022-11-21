namespace Profitable.Services.COT
{
	using AutoMapper;
	using Microsoft.EntityFrameworkCore;
	using Profitable.Common.GlobalConstants;
	using Profitable.Common.Models.ScrapersModels;
	using Profitable.Common.Scraper;
	using Profitable.Common.Services;
	using Profitable.Data.Repository.Contract;
	using Profitable.Models.EntityModels;
	using Profitable.Models.RequestModels.COT;
	using Profitable.Models.ResponseModels.COT;
	using Profitable.Services.COT.Contracts;

	public class COTService : ICOTService
	{
		private readonly IMapper mapper;
		private readonly IRepository<COTReport> cotReportRepository;

		public COTService(
			IMapper mapper,
			IRepository<COTReport> cotReportRepository)
		{
			this.mapper = mapper;
			this.cotReportRepository = cotReportRepository;
		}

		public async Task<GetCOTResponseModel> GetReport(
			GetCOTRequestModel getCOTRequestModel)
		{
			var latestCOT = await cotReportRepository
				.GetAllAsNoTracking()
				.FirstOrDefaultAsync(report =>
					report.COTReportedInstrumentId == getCOTRequestModel.InstrumentGuid &&
					report.DatePublished == getCOTRequestModel.FromDate);

			latestCOT ??= await PrepareNewReport(getCOTRequestModel);

			///Issue - Potential Error - Scraping - GitHub
			// latestCOT ??= 

			return mapper.Map<GetCOTResponseModel>(latestCOT);
		}

		private async Task<COTReport> PrepareNewReport(
			GetCOTRequestModel getCOTRequestModel)
		{
			var newReport = await ScrapeNewReport(
					GlobalServicesConstants.CotReportSourcesLinks[getCOTRequestModel.InstrumentName],
					getCOTRequestModel.FromDate);

			var latestCOT = mapper.Map<COTReport>(
					   newReport,
					   opt => opt.AfterMap((src, dest) =>
					   {
						   dest.COTReportedInstrumentId = getCOTRequestModel.InstrumentGuid;
					   }));

			await cotReportRepository.AddAsync(latestCOT);

			await cotReportRepository.SaveChangesAsync();

			return latestCOT;
		}

		private async Task<ScrapeBigMoneyPositionsModel> ScrapeNewReport(
			string link,
			DateTime cotDate)
		{
			var tuesday = new List<DateTime>()
			{
				UtilityMethods.GetTheLastDayOfWeekFromDate(DayOfWeek.Tuesday, cotDate)
			};

			return (await TradingsterScraper.ScrapeBigMoneyPositions(link, tuesday))[0];
		}
	}
}
