using AutoMapper;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Journals;
using Profitable.Models.ResponseModels.Journals;

namespace Profitable.Common.Automapper.Profiles
{
	public class JournalMapper : Profile
	{
		public JournalMapper()
		{
			CreateMap<Journal, JournalResponseModel>();
			CreateMap<UpdateJournalRequestModel, Journal>();
		}
	}
}
