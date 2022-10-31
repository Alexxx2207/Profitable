using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.ResponseModels.Futures;
using Profitable.Services.Futures.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Services.Futures
{
	public class FuturesService : IFuturesService
	{
		private readonly IRepository<FuturesContract> futuresContractRepository;
		private readonly IMapper mapper;

		public FuturesService(
			IRepository<FuturesContract> futuresContractRepository,
			IMapper mapper)
		{
			this.futuresContractRepository = futuresContractRepository;
			this.mapper = mapper;
		}

		public async Task<List<FuturesContractsResponseModel>> GetAllFuturesContracts()
		{
			return await futuresContractRepository
					.GetAllAsNoTracking()
					.OrderBy(fc => fc.Name)
					.Select(fc => mapper.Map<FuturesContractsResponseModel>(fc))
					.ToListAsync();
		}
	}
}
