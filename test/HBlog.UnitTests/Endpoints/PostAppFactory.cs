using HBlog.Application;
using HBlog.Application.Services;
using HBlog.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System;
using HBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace HBlog.UnitTests.Endpoints
{
    public class PostAppFactory : WebApplicationFactory<Program>
    {
        public Mock<IPostService> _mockPostService { get; }
        public Mock<IPostRepository> _mockPostRepository { get; }
        public Mock<IUserService> _mockUserService { get; }

        public PostAppFactory()
        {
            _mockPostService = new Mock<IPostService>();
            _mockPostRepository = new Mock<IPostRepository>();
            _mockUserService = new Mock<IUserService>();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("Environment", "test");
            builder.UseContentRoot(".");
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(_mockPostService.Object);
                services.AddSingleton(_mockPostRepository.Object);
                services.AddSingleton(_mockUserService.Object);
            });
        }

    }
}
