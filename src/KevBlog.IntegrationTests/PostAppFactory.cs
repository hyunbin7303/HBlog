using KevBlog.Application.Services;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;
namespace KevBlog.IntegrationTests
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
