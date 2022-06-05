using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Data.Seeding
{
    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
    }
}
