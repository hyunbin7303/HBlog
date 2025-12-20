using HBlog.Application.Services;
using HBlog.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Moq;

namespace HBlog.IntegrationTests.Base
{
    public class PostAppFactory : WebApplicationFactory<Program>
    {
        public Mock<IPostRepository> _mockPostRepository { get; }
        public Mock<IUserService> _mockUserService { get; }

        public PostAppFactory()
        {
            _mockPostRepository = new Mock<IPostRepository>();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                //services.RemoveAll(typeof(DbContextOptions<DataContext>));
                //services.GetRequired<DataContext>();
            });
        }

    }
}
