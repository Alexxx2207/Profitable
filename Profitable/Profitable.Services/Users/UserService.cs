using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.ViewModels.Users;
using Profitable.Services.Users.Contracts;

namespace Profitable.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IRepository<ApplicationUser> repository;
        private readonly IMapper mapper;

        public UserService(IRepository<ApplicationUser> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<UserDetailsViewModel> GetUserDetailsAsync(string username)
        {
            var user = await repository
                .GetAllAsNoTracking()
                .FirstAsync(user => user.UserName == username);

            return mapper.Map<UserDetailsViewModel>(user);
        }
    }
}
