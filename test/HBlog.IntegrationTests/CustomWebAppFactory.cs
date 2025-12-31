using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HBlog.IntegrationTests
{
    public class CustomWebAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("test");
            builder.ConfigureServices(services =>
            {
                // Remove IdentityContext
                var identityContext = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IdentityContext));
                if (identityContext != null)
                {
                    services.Remove(identityContext);
                }
                
                // Remove BlogContext
                var blogContext = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(BlogContext));
                if (blogContext != null)
                {
                    services.Remove(blogContext);
                }
                
                // Remove DbContextOptions
                var options = services.Where(r => r.ServiceType == typeof(DbContextOptions)
                                              || r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>)).ToArray();
                foreach (var option in options)
                {
                    services.Remove(option);
                }

                // Add in-memory databases for both contexts
                services.AddDbContext<IdentityContext>(options =>
                {
                    options.UseInMemoryDatabase("HBlogIdentityInMemory");
                });
                
                services.AddDbContext<BlogContext>(options =>
                {
                    options.UseInMemoryDatabase("HBlogInMemory");
                });
            });
        }
    }
}
