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
		private readonly IRepository<COTReportedInstrument> cotReportedInstrumentRepository;

		public COTService(
			IMapper mapper,
			IRepository<COTReport> cotReportRepository,
			IRepository<COTReportedInstrument> cotReportedInstrumentRepository)
		{
			this.mapper = mapper;
			this.cotReportRepository = cotReportRepository;
			this.cotReportedInstrumentRepository = cotReportedInstrumentRepository;
		}

		public async Task<COTReportResponseModel> GetReport(
			GetCOTRequestModel getCOTRequestModel)
		{
			try
			{
                if (getCOTRequestModel.FromDate.Date > DateTime.UtcNow.Date)
                {
                    getCOTRequestModel.FromDate = UtilityMethods.GetTheLastDayOfWeekFromDate(
                        DayOfWeek.Tuesday,
                        DateTime.UtcNow.Date);
                }
                else
                {
                    getCOTRequestModel.FromDate = UtilityMethods.GetTheLastDayOfWeekFromDate(
                        DayOfWeek.Tuesday,
                        getCOTRequestModel.FromDate);
                }

                var latestCOT = await GetFromDB(getCOTRequestModel)
                                ??
                                await PrepareNewReport(getCOTRequestModel);

                if (latestCOT == null)
                {
                    do
                    {
                        getCOTRequestModel.FromDate = getCOTRequestModel.FromDate.AddDays(-7);
                        latestCOT = await GetFromDB(getCOTRequestModel)
                                    ??
                                    await PrepareNewReport(getCOTRequestModel);

                    } while (latestCOT == null);
                }

                return mapper.Map<COTReportResponseModel>(latestCOT);
            }
			catch (Exception)
			{
				throw new Exception(GlobalServicesConstants.EntityDoesNotExist("Finantial Instrument"));
			}
			
		}

		public async Task<IEnumerable<COTReportedInstrumentResponseModel>> GetReportedInstruments()
		{
			try
			{
				var instruments = (await cotReportedInstrumentRepository
					.GetAllAsNoTracking()
					.ToListAsync())
					.Select(instrument =>
							mapper.Map<COTReportedInstrumentResponseModel>(instrument));

				return instruments;
			}
			catch (Exception)
			{
				throw;
			}
        }

		private async Task<COTReport> GetFromDB(GetCOTRequestModel getCOTRequestModel)
		{
            return await cotReportRepository
                .GetAllAsNoTracking()
                .FirstOrDefaultAsync(report =>
                    report.COTReportedInstrumentId == getCOTRequestModel.InstrumentGuid &&
                    report.DatePublished == getCOTRequestModel.FromDate);
        }

		private async Task<COTReport> PrepareNewReport(
			GetCOTRequestModel getCOTRequestModel)
		{
			var newReport = await ScrapeNewReport(
					GlobalDatabaseConstants.CotReportSourcesLinks[getCOTRequestModel.InstrumentName],
					getCOTRequestModel.FromDate);

			if(newReport == null)
			{
				return null;
			}

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
                cotDate
			};

			var reports =  await TradingsterScraper.ScrapeBigMoneyPositions(link, tuesday);

            return reports.Count > 0 ? reports[0] : null;
		}
	}
}
