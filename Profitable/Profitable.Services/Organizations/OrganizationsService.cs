namespace Profitable.Services.Organizations
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Profitable.Common.Enums;
    using Profitable.Common.GlobalConstants;
    using Profitable.Common.Models;
    using Profitable.Data.Repository.Contract;
    using Profitable.Models.EntityModels;
    using Profitable.Models.RequestModels.Organizations;
    using Profitable.Services.Organizations.Contracts;

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

		public async Task<Result> AddMembersToOrganization(AddMembersRequestModel model)
		{
			try
			{
				await usersRepository
					.GetAll()
					.Where(u => model.Members.Any(m => m == u.Guid))
					.ExecuteUpdateAsync(s =>
						s.SetProperty(x => x.OrganizationId, x => model.OrganizationId)
						 .SetProperty(x => x.OrganizationRole, x => UserOrganizationsRoles.Member));

				return true;
            }
			catch (Exception e)
			{
				return e.Message;
			}
            
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
				if(user.OrganizationId.HasValue)
				{
                    throw new InvalidOperationException(
                        "User is already in a organization! " +
						"User can be in only one organization at a time. " +
						"Make sure you are not in one first.");
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
