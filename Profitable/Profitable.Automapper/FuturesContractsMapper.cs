namespace Profitable.Common.Automapper
{
	using AutoMapper;
	using Profitable.Models.EntityModels;
	using Profitable.Models.ResponseModels.Futures;

	public class FuturesContractsMapper : Profile
	{
		public FuturesContractsMapper()
		{
			CreateMap<FuturesContract, FuturesContractsResponseModel>();
		}
	}
}
