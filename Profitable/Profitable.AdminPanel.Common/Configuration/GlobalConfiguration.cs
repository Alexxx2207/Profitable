namespace Profitable.AdminPanel
{
	using Microsoft.Data.SqlClient;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Profitable.Data;

	public static class GlobalConfiguration
	{
		public static ApplicationDbContext CreateDbContext()
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
			var connectionString = new SqlConnectionStringBuilder(
					configuration.GetConnectionString("DefaultConnection"));

			builder.UseSqlServer(connectionString.ConnectionString);

			return new ApplicationDbContext(builder.Options);

		}
	}
}
