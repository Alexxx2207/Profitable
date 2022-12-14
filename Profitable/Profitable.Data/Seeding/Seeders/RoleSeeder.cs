namespace Profitable.Data.Seeding.Seeders
{
	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.DependencyInjection;
	using Profitable.Common.Enums;
	using Profitable.Data.Seeding.Seeders.Contracts;

	public class RoleSeeder : ISeeder
	{
		public async Task SeedAsync(
			ApplicationDbContext dbContext,
			IServiceProvider serviceProvider)
		{
			var roleManager =
					serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

			foreach (UserOrganizationsRoles role in Enum.GetValues(typeof(UserOrganizationsRoles)))
			{
				await SeedRoleAsync(roleManager, role.ToString());
			}
		}

		private static async Task SeedRoleAsync(
			RoleManager<IdentityRole<Guid>> roleManager,
			string roleName)
		{
			var role = await roleManager.FindByNameAsync(roleName);
			if (role == null)
			{
				var result = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
				if (!result.Succeeded)
				{
					throw new Exception(
						string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
				}
			}
		}
	}
}
