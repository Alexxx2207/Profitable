namespace Profitable.Models.RequestModels.Organizations
{
	public class RemoveMemberRequestModel
	{
		public Guid RequesterId { get; set; }

		public Guid MemberToRemoveId { get; set; }
	}
}
