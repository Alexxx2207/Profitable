using Profitable.Models.EntityModels.EntityBaseClass;

namespace Profitable.Models.EntityModels
{
	public class Organization : EntityBase
	{
		public Organization()
		{
			Users = new HashSet<ApplicationUser>();
		}

		public string Name { get; set; }

		public ICollection<ApplicationUser> Users { get; set; }
	}
}
