namespace Profitable.Data.Seeding.Seeders.Contracts
{
    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider = null);
    }
}
