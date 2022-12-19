using Profitable.Common.Enums;

namespace Profitable.Models.RequestModels.Organizations
{
	public class ChangeMemberRoleRequestModel
	{
		public Guid OwnerId { get; set; }

		public Guid ManipulatedMemberId { get; set; }

		public UserOrganizationsRoles RoleToAssign { get; set; }
	}
}
