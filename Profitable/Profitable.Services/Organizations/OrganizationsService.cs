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
	using Profitable.Models.ResponseModels.Organizations;
	using Profitable.Services.Organizations.Contracts;
	using System.Data;

	public class OrganizationsService : IOrganizationsService, IOrganizationMembersService
	{
		private readonly IMapper mapper;
		private readonly IRepository<Organization> organizationRepository;
		private readonly IRepository<OrganizationMessage> organizationMessagesRepository;
		private readonly IRepository<ApplicationUser> usersRepository;

		public OrganizationsService(
			IMapper mapper,
			IRepository<Organization> organizationRepository,
			IRepository<OrganizationMessage> organizationMessagesRepository,
			IRepository<ApplicationUser> usersRepository)
		{
			this.mapper = mapper;
			this.organizationRepository = organizationRepository;
			this.organizationMessagesRepository = organizationMessagesRepository;
			this.usersRepository = usersRepository;
		}

		public async Task<Result> AddMemberToOrganization(AddMemberRequestModel model)
		{
			try
			{
				var requester = await usersRepository
					.GetAllAsNoTracking()
					.Where(u => !u.IsDeleted)
					.FirstOrDefaultAsync(u => u.Guid == model.RequesterId);

				if (requester == null)
				{
					throw new InvalidOperationException(
						GlobalServicesConstants.EntityDoesNotExist("Requester"));
				}

				if (requester.OrganizationRole != UserOrganizationsRoles.Owner &&
					requester.OrganizationRole != UserOrganizationsRoles.Admin)
				{
					throw new UnauthorizedAccessException("Requester is not authorized");
				}

				var organization = await organizationRepository
					.GetAll()
					.Where(o => !o.IsDeleted)
					.FirstOrDefaultAsync(o => o.Guid == model.OrganizationId);

				if (organization == null)
				{
					throw new InvalidOperationException(
						GlobalServicesConstants.EntityDoesNotExist("Organization"));
				}

				await usersRepository
					.GetAll()
					.Where(u => !u.IsDeleted && model.MemberId == u.Guid)
					.ExecuteUpdateAsync(s =>
						s.SetProperty(x => x.OrganizationId, x => model.OrganizationId)
						 .SetProperty(x => x.OrganizationRole, x => UserOrganizationsRoles.Member));

				return true;
			}
			catch (InvalidOperationException e)
			{
				return e.Message;
			}
			catch (UnauthorizedAccessException e)
			{
				return e.Message;
			}
			catch (Exception)
			{
				return GlobalServicesConstants.InternalServerErrorMessage;
			}
		}

		public async Task<Result> AddOrganization(AddOrganizationRequestModel model)
		{
			try
			{
				var user = await usersRepository
					.GetAll()
					.Where(u => !u.IsDeleted)
					.FirstOrDefaultAsync(u => u.Guid == model.RequesterId);

				if (user == null)
				{
					throw new InvalidOperationException(
						GlobalServicesConstants.EntityDoesNotExist("Requester"));
				}
				if (user.OrganizationId.HasValue)
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
					user.OrganizationRole = UserOrganizationsRoles.Owner;

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
				return GlobalServicesConstants.InternalServerErrorMessage;
			}
		}

		public async Task<Result> AddMessageInOrganization(AddMessageRequestModel model)
		{
			try
			{
				var sender = await usersRepository
					.GetAll()
					.Where(u => !u.IsDeleted)
					.FirstOrDefaultAsync(u =>
						u.Guid == model.SenderId &&
						u.OrganizationId == model.OrganizationId);

				if (sender == null)
				{
					throw new InvalidOperationException(
						"Sender does not exist of in not in the organization!");
				}
				var organizationMessage = mapper.Map<OrganizationMessage>(model);

				await organizationMessagesRepository.AddAsync(organizationMessage);

				await organizationMessagesRepository.SaveChangesAsync();

				return true;
			}
			catch (InvalidOperationException e)
			{
				return e.Message;
			}
			catch (Exception)
			{
				return GlobalServicesConstants.InternalServerErrorMessage;
			}
		}

		public async Task<Result> ChangeMemberRoleToOrganization(ChangeMemberRoleRequestModel model)
		{
			try
			{
				var owner = await usersRepository
				.GetAllAsNoTracking()
				.Where(u => !u.IsDeleted)
				.FirstOrDefaultAsync(u => u.Guid == model.RequesterId);

				if (owner == null)
				{
					throw new InvalidOperationException(
						GlobalServicesConstants.EntityDoesNotExist("Requester"));
				}

				if (owner.OrganizationRole != UserOrganizationsRoles.Owner &&
					owner.OrganizationRole != UserOrganizationsRoles.Admin)
				{
					throw new UnauthorizedAccessException("Requester is not authorized");
				}

				var memberToManipulate = await usersRepository
					.GetAll()
					.Where(u => !u.IsDeleted)
					.FirstOrDefaultAsync(u =>
						u.Guid == model.ManipulatedMemberId &&
						u.OrganizationId == owner.OrganizationId);

				if (memberToManipulate == null)
				{
					throw new InvalidOperationException(
						GlobalServicesConstants.EntityDoesNotExist("Member to manipulate"));
				}

				if (!Enum.TryParse<UserOrganizationsRoles>(model.RoleToAssign, true, out var role))
				{
					throw new InvalidOperationException(
					   GlobalServicesConstants.EntityDoesNotExist("The role to assign"));
				}

				memberToManipulate.OrganizationRole = role;

				await usersRepository.SaveChangesAsync();

				return true;
			}
			catch (InvalidOperationException e)
			{
				return e.Message;
			}
			catch (UnauthorizedAccessException e)
			{
				return e.Message;
			}
			catch (Exception)
			{
				return GlobalServicesConstants.InternalServerErrorMessage;
			}
		}

		public async Task<Result> DeleteOrganization(DeleteOrganizationRequestModel model)
		{
			try
			{
				var requester = await usersRepository
					.GetAllAsNoTracking()
					.Where(u => !u.IsDeleted)
					.FirstOrDefaultAsync(u => u.Guid == model.RequesterId);

				if (requester == null)
				{
					throw new InvalidOperationException(
						GlobalServicesConstants.EntityDoesNotExist("Requester"));
				}

				if (requester.OrganizationRole != UserOrganizationsRoles.Owner)
				{
					throw new UnauthorizedAccessException("Requester is not authorized");
				}

				var organization = await organizationRepository
					.GetAll()
					.Where(o => !o.IsDeleted)
					.FirstOrDefaultAsync(o => o.Guid == model.OrganizationId);

				if (organization == null)
				{
					throw new InvalidOperationException(
						GlobalServicesConstants.EntityDoesNotExist("Organization"));
				}

				var membersInOrganization = await usersRepository
					.GetAll()
					.Where(u => !u.IsDeleted && u.OrganizationId == model.OrganizationId)
					.ExecuteUpdateAsync(s =>
						s.SetProperty(x => x.OrganizationId, x => null)
						 .SetProperty(x => x.OrganizationRole, x => UserOrganizationsRoles.None));

				organizationRepository.Delete(organization);

				await usersRepository.SaveChangesAsync();
				await organizationRepository.SaveChangesAsync();

				return true;
			}
			catch (InvalidOperationException e)
			{
				return e.Message;
			}
			catch (UnauthorizedAccessException e)
			{
				return e.Message;
			}
			catch (Exception)
			{
				return GlobalServicesConstants.InternalServerErrorMessage;
			}
		}

		public async Task<Result> RemoveMemberFromOrganization(RemoveMemberRequestModel model)
		{
			try
			{
				var requester = await usersRepository
					.GetAllAsNoTracking()
					.Where(u => !u.IsDeleted)
					.FirstOrDefaultAsync(u => u.Guid == model.RequesterId);

				if (requester == null)
				{
					throw new InvalidOperationException(
						GlobalServicesConstants.EntityDoesNotExist("Requester"));
				}

				if (requester.OrganizationRole != UserOrganizationsRoles.Owner &&
					requester.OrganizationRole != UserOrganizationsRoles.Admin)
				{
					throw new UnauthorizedAccessException("Requester is not authorized");
				}

				var memberToRemove = await usersRepository
					.GetAll()
					.FirstOrDefaultAsync(u =>
						!u.IsDeleted &&
						u.Guid == model.MemberToRemoveId &&
						u.OrganizationId == requester.OrganizationId);

				if (memberToRemove == null)
				{
					throw new InvalidOperationException(
						GlobalServicesConstants.EntityDoesNotExist("Member to remove"));
				}

				if (memberToRemove.OrganizationRole == UserOrganizationsRoles.Owner)
				{
					throw new InvalidOperationException("Owner cannot be removed!");
				}

				memberToRemove.OrganizationId = null;
				memberToRemove.OrganizationRole = UserOrganizationsRoles.None;

				await usersRepository.SaveChangesAsync();

				return true;
			}
			catch (InvalidOperationException e)
			{
				return e.Message;
			}
			catch (UnauthorizedAccessException e)
			{
				return e.Message;
			}
			catch (Exception)
			{
				return GlobalServicesConstants.InternalServerErrorMessage;
			}
		}

		public async Task<Result> UpdateOrganizationGeneralSettings(UpdateOrganizationRequestModel model)
		{
			try
			{
				var requester = await usersRepository
					.GetAllAsNoTracking()
					.Where(u => !u.IsDeleted)
					.FirstOrDefaultAsync(u =>
						u.Guid == model.RequesterId &&
						u.OrganizationId == model.OrganizationIdToUpdate);

				if (requester == null)
				{
					throw new InvalidOperationException(
						GlobalServicesConstants.EntityDoesNotExist("Requester"));
				}

				if (requester.OrganizationRole != UserOrganizationsRoles.Owner &&
					requester.OrganizationRole != UserOrganizationsRoles.Admin)
				{
					throw new UnauthorizedAccessException("Requester is not authorized");
				}

				var organization = await organizationRepository
					 .GetAllAsNoTracking()
					 .Where(o => !o.IsDeleted)
					 .FirstOrDefaultAsync(o => o.Guid == model.OrganizationIdToUpdate);

				if (organization == null)
				{
					throw new InvalidOperationException(
						GlobalServicesConstants.EntityDoesNotExist("Organization"));
				}

				var newOrganization = mapper.Map<Organization>(model);

				newOrganization.Guid = organization.Guid;

				organizationRepository.Update(newOrganization);

				await organizationRepository.SaveChangesAsync();

				return true;
			}
			catch (InvalidOperationException e)
			{
				return e.Message;
			}
			catch (UnauthorizedAccessException e)
			{
				return e.Message;
			}
			catch (Exception)
			{
				return GlobalServicesConstants.InternalServerErrorMessage;
			}
		}

		public async Task<List<OrganizationMessageResponseModel>> GetOrganizationMessages(
			GetOrganizationMessagesRequestModel model)
		{

			var requester = await usersRepository
				.GetAllAsNoTracking()
				.Where(u => !u.IsDeleted)
				.FirstOrDefaultAsync(u =>
					u.Guid == model.RequesterId &&
					u.OrganizationId == model.OrganizationId);

			if (requester == null)
			{
				throw new InvalidOperationException(
					"Requester does not exist or is not in the organization!");
			}

			var organizationMessages = await organizationMessagesRepository
				 .GetAllAsNoTracking()
				 .Include(om => om.Sender)
				 .Where(om =>
					!om.IsDeleted &&
					om.OrganizationId == model.OrganizationId)
				 .OrderByDescending(om => om.SentOn)
				 .Skip(model.Page * model.PageCount)
				 .Take(model.PageCount)
				 .ToListAsync();

			return mapper.Map<List<OrganizationMessageResponseModel>>(organizationMessages);
		}

		public async Task<OrganizationResponseModel> GetOrganization(Guid organizationId)
		{
			var organization = await organizationRepository
				.GetAllAsNoTracking()
				.FirstOrDefaultAsync(x => x.Guid == organizationId);

			return mapper.Map<OrganizationResponseModel>(organization);
		}
	}
}
