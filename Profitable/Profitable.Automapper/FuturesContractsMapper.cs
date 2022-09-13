using AutoMapper;
using Profitable.Models.EntityModels;
using Profitable.Models.ResponseModels.Futures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Automapper
{
	public class FuturesContractsMapper : Profile
	{
		public FuturesContractsMapper()
		{
			CreateMap<FuturesContract, FuturesContractsResponseModel>();
		}
	}
}
