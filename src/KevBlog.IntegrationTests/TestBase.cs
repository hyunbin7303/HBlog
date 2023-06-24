using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace KevBlog.IntegrationTests
{
    public class WebApplicationFactory : WebApplicationFactory<Program>
    {

    }

    public abstract class TestBase
    {
        protected IConfiguration _config;
        public TestBase()
        {
            _config = new ConfigurationBuilder().AddJsonFile(@"appsettings.json", optional: false, true).Build();
        }
    }
}