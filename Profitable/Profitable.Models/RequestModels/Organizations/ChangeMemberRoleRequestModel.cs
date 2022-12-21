using Profitable.Common.Enums;

namespace Profitable.Models.RequestModels.Organizations
{
	public class ChangeMemberRoleRequestModel
	{
		public Guid RequesterId { get; set; }

		public Guid ManipulatedMemberId { get; set; }

		public UserOrganizationsRoles RoleToAssign { get; set; }
	}
}
