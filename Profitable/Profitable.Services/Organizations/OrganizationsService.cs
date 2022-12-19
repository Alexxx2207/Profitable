using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Profitable.Common.GlobalConstants;
using Profitable.Common.Models;
using Profitable.Data.Repository.Contract;
using Profitable.Models.EntityModels;
using Profitable.Models.RequestModels.Organizations;
using Profitable.Services.Organizations.Contracts;

namespace Profitable.Services.Organizations
{
	public class OrganizationsService : IOrganizationsService, IMembersService
	{
		private readonly IMapper mapper;
		private readonly IRepository<Organization> organizationRepository;
		private readonly IRepository<ApplicationUser> usersRepository;

		public OrganizationsService(
			IMapper mapper,
			IRepository<Organization> organizationRepository,
			IRepository<ApplicationUser> usersRepository)
		{
			this.mapper = mapper;
			this.organizationRepository = organizationRepository;
			this.usersRepository = usersRepository;
		}

		public Task<Result> AddMembersToOrganization(AddMembersRequestModel addMembersRequestModel)
		{
			throw new NotImplementedException();
		}

		public async Task<Result> AddOrganization(AddOrganizationRequestModel model)
		{
			try
			{
				var user = await usersRepository
					.GetAll()
					.FirstOrDefaultAsync(u => u.Guid == model.OwnerId);

				if (user == null)
				{
					throw new InvalidOperationException(
						GlobalServicesConstants.EntityDoesNotExist("User"));
				}
				else
				{
					var organization = new Organization()
					{
						Name = model.Name,
					};

					user.OrganizationId = organization.Guid;
					user.OrganizationRole = Profitable.Common.Enums.UserOrganizationsRoles.Owner;

					await organizationRepository.AddAsync(organization);

					await organizationRepository.SaveChangesAsync();
					await usersRepository.SaveChangesAsync();
				}

				return true;
			}
			catch (InvalidOperationException e)
			{
				return e.Message;
			}
			catch (Exception)
			{
				return "Internal Server Error";
			}
		}

		public Task<Result> ChangeMemberRoleToOrganization(ChangeMemberRoleRequestModel changeMemberRoleRequestModel)
		{
			throw new NotImplementedException();
		}

		public Task<Result> DeleteOrganization(Guid organization)
		{
			throw new NotImplementedException();
		}

		public Task<Result> RemoveMemberToOrganization(RemoveMemberRequestModel removeMemberRequestModel)
		{
			throw new NotImplementedException();
		}

		public Task<Result> UpdateOrganizationGeneralSettings(UpdateOrganizationRequestModel model)
		{
			throw new NotImplementedException();
		}
	}
}
