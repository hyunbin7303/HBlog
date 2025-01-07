using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HBlog.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace HBlog.IntegrationTests
{
    
    public class GlobalExceptionHandlerTest
    {

        [SetUp]
        public void Init()
        {

        }

        [Test]
        public async Task MiddlewareTest_ReturnsError()
        {
            //builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddExceptionHandler<GlobalExceptionHandler>();
                        })
                        .Configure(app =>
                        {
                        });
                })
                .StartAsync();

            var response = await host.GetTestClient().GetAsync("/api/posts");

        }
    }
}
