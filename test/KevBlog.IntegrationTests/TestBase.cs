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
}