using HBlog.Application;
using HBlog.Application.Services;
using HBlog.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System;
namespace HBlog.IntegrationTests
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
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                //services.RemoveAll(typeof(DbContextOptions<DataContext>));
                //services.GetRequiredService<DataContext>();
                services.AddSingleton(_mockPostService.Object);
                services.AddSingleton(_mockPostRepository.Object);
            });
        }

    }
}
