using HBlog.Application.Services;
using HBlog.Domain.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;

namespace HBlog.IntegrationTests.Base
{
    public class MessageAppFactory : WebApplicationFactory<Program>
    {
        public Mock<IMessageService> _mockMessageService { get; }
        public Mock<IMessageRepository> _mockMessageRepository { get; }
        public Mock<IUserService> _mockUserService { get; }

        public MessageAppFactory()
        {
            _mockMessageService = new();
            _mockMessageRepository = new();
            _mockUserService = new();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {

                services.AddAuthentication("TestScheme").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

                services.AddSingleton(_mockMessageService.Object);
                services.AddSingleton(_mockMessageRepository.Object);
            });
        }
    }
}
