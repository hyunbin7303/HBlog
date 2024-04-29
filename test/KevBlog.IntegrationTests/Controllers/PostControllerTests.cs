using Moq;
using System.Net;
using System.Text.Json;
using KevBlog.Contract.DTOs;
using KevBlog.Contract.Common;
using KevBlog.Domain.Common.Params;

namespace KevBlog.IntegrationTests.Controllers
{
    //https://github.com/hassanhabib/SchoolEM/blob/master/SchoolEM.Acceptance.Tests/Brokers/ApiTestCollection.cs
    //https://andrewlock.net/exploring-dotnet-6-part-6-supporting-integration-tests-with-webapplicationfactory-in-dotnet-6/

    public class PostControllerTests : IDisposable
    {
        private PostAppFactory _factory;
        private HttpClient _client;

        public PostControllerTests()
        {
            _factory = new PostAppFactory();
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GivenValidPosts_WhenGetPostsCalled_ThenResponsePosts()
        {
            PostParams postParams = new();
            IEnumerable<PostDisplayDto> posts = new List<PostDisplayDto>
            {
                new PostDisplayDto { Id = 1, Title = "PostDisplay#1", Desc = "TestingDesc1", Content = "TestingContent1", UserName="hyunbin7303" },
                new PostDisplayDto { Id = 2, Title = "PostDisplay#2", Desc = "TestingDesc2", Content = "TestingContent2", UserName="hyunbin7303" },
            };
            _factory._mockPostService.Setup(x => x.GetPosts(postParams)).ReturnsAsync(posts);
            var response = await _client.GetAsync("/api/posts");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true 
            };
            var data = JsonSerializer.Deserialize<IEnumerable<PostDisplayDto>>(await response.Content.ReadAsStringAsync(), options);
            Assert.Collection((data as IEnumerable<PostDisplayDto>)!,
                r=>
                {
                    Assert.Equal("hyunbin7303", r.UserName);
                    Assert.Equal("PostDisplay#1", r.Title);
                    Assert.Equal("TestingContent1", r.Content);
                },
                r=>
                {
                    Assert.Equal("hyunbin7303", r.UserName);
                    Assert.Equal("PostDisplay#2", r.Title);
                    Assert.Equal("TestingContent2", r.Content);
                });
        }


        [Fact]
        public async Task GivenValidPostId_WhenGetByIdCalled_ThenReturnServiceResult()
        {
            var post = new PostDisplayDetailsDto { Id = 1, Title = "PostDisplay#1", Desc = "TestingDesc1", Content = "TestingContent1", UserName = "hyunbin7303" };
            var serviceResult = ServiceResult.Success(post);
            _factory._mockPostService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(serviceResult);

            var response = await _client.GetAsync("/api/posts/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
            var result = JsonSerializer.Deserialize<PostDisplayDetailsDto>(await response.Content.ReadAsStringAsync(), options);
            Assert.Equal(post, serviceResult.Value);
            
        }


        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
