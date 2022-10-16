using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common.GlobalConstants;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.ResponseModels.Users;
using Profitable.Services.Search.Contracts;

namespace Profitable.Services.Search
{
	public class UserSearchService : IUserSearchService
	{
		private readonly IRepository<ApplicationUser> repository;
		private readonly IMapper mapper;

		public UserSearchService(IRepository<ApplicationUser> repository, IMapper mapper)
		{
			this.repository = repository;
			this.mapper = mapper;
		}

		public async Task<List<UserDetailsResponseModel>> GetMatchingUsers(string searchTerm)
		{
			searchTerm = searchTerm.ToLower();

			var users = await repository
				.GetAllAsNoTracking()
				.Where(u => !u.IsDeleted)
				.Where(u =>
					(u.FirstName + " " + u.LastName).ToLower().Contains(searchTerm)
					||
					u.Email.ToLower().Contains(searchTerm))
				.Take(GlobalServicesConstants.SearchResultsCountToTake)
				.Select(user => mapper.Map<UserDetailsResponseModel>(user))
				.ToListAsync();

			return users;
		}
	}
}
