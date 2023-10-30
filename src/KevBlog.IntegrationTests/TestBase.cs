using KevBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KevBlog.IntegrationTests
{
    public abstract class TestBase
    {
        protected IConfiguration _config;
        public TestBase()
        {
            _config = new ConfigurationBuilder().AddJsonFile(@"appsettings.json", optional: false, true).Build();
        }
    }
    public abstract class IntegrationTestBase : TestBase
    {
        protected readonly DataContext _dataContext;

        public IntegrationTestBase()
        {
            var check = _config.GetConnectionString("DefaultConnection");
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(check).Options;
            _dataContext = new DataContext(options);
        }
    }
}