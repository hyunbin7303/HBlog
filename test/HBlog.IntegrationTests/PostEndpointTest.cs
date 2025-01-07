using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace HBlog.IntegrationTests
{
    public class PostEndpointTest
    {
        private CustomWebAppFactory<Program> factory;
        private HttpClient _client;


        public PostEndpointTest()
        {
            factory = new CustomWebAppFactory<Program>();
            _client = factory.CreateClient();
        }

        [Test]
        public async Task Given_When_Then()
        {
            //_factory._mockUserRepository.Setup(o => o.GetUserByIdAsync(1))
            //    .ReturnsAsync(new User() { Id = 1, UserName = "test" });

            var response = await _client.GetAsync("/api/users/random");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

    }
}
