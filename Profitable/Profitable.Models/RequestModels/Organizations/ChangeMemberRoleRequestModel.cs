namespace Profitable.Models.RequestModels.Organizations
{
	public class ChangeMemberRoleRequestModel
	{
		public Guid RequesterId { get; set; }

		public Guid ManipulatedMemberId { get; set; }

		public string RoleToAssign { get; set; }
	}
}
