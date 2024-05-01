using HBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HBlog.IntegrationTests
{
    public abstract class IntegrationTestBase : TestBase
    {
        protected readonly DataContext _dataContext;

        public IntegrationTestBase()
        {
            var check = _config.GetConnectionString("DefaultConnection");
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql(check).Options;
            _dataContext = new DataContext(options);
        }
    }
}