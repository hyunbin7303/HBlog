using HBlog.Application.Services;
using HBlog.Domain.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
namespace HBlog.UnitTests.Endpoints
{
    public class UserAppFactory : WebApplicationFactory<Program>
    {
        public Mock<IUserService> _mockUserService { get; }
        public Mock<IUserRepository> _mockUserRepository { get; }
        public int DefaultUserId { get; set; } = 1;

        public UserAppFactory()
        {
            _mockUserService = new Mock<IUserService>();
            _mockUserRepository = new Mock<IUserRepository>();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("Environment", "test");
            builder.UseContentRoot(".");
            builder.ConfigureTestServices(services =>
            {
                services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = DefaultUserId);
                services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                    .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, _ => { });
                services.AddLogging(builder => builder.ClearProviders().AddConsole().AddDebug());

                services.AddSingleton(_mockUserService.Object);
                services.AddSingleton(_mockUserRepository.Object);

            });
        }
    }
}
