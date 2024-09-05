using System.Collections;
using Moq;
using System.Net;
using System.Text.Json;
using HBlog.Contract.DTOs;
using HBlog.Contract.Common;
using HBlog.Domain.Common.Params;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Assert = NUnit.Framework.Assert;

namespace HBlog.UnitTests.Endpoints
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

        [Test]
        public async Task GivenValidPosts_WhenGetPostsCalled_ThenResponsePosts()
        {
            IEnumerable<PostDisplayDto> posts = new List<PostDisplayDto>
            {
                new() { Id = 1, Title = "PostDisplay#1", Desc = "TestingDesc1", Content = "TestingContent1", UserName="hyunbin7303" },
                new() { Id = 2, Title = "PostDisplay#2", Desc = "TestingDesc2", Content = "TestingContent2", UserName="hyunbin7303" },
            };
            _factory._mockPostService.Setup(x => x.GetPosts(It.IsAny<PostParams>())).ReturnsAsync(posts);
            var response = await _client.GetAsync("/api/posts");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };


            var data = JsonSerializer.Deserialize<ApiResponse<IEnumerable<PostDisplayDto>>>(await response.Content.ReadAsStringAsync(), options);
            IEnumerable<PostDisplayDto> resultPosts = data.Data;

            CollectionAssert.AllItemsAreNotNull(resultPosts);
            //CollectionAssert.AreEqual(posts, resultPosts);
        }


        [Test]
        public async Task GivenValidPostId_WhenGetByIdCalled_ThenReturnServiceResult()
        {
            var post = new PostDisplayDetailsDto { Id = 1, Title = "PostDisplay#1", Desc = "TestingDesc1", Content = "TestingContent1", UserName = "hyunbin7303" };
            var serviceResult = ServiceResult.Success(post);
            _factory._mockPostService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(serviceResult);

            var response = await _client.GetAsync("/api/posts/1");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
            var result = JsonSerializer.Deserialize<PostDisplayDetailsDto>(await response.Content.ReadAsStringAsync(), options);
            Assert.That(result.Id, Is.EqualTo(post.Id));
            Assert.That(result.UserName, Is.EqualTo(post.UserName));
            Assert.That(result.Title, Is.EqualTo(post.Title));
            Assert.That(result.Desc, Is.EqualTo(post.Desc));
            Assert.That(result.Content, Is.EqualTo(post.Content));
        }

        [Test]
        public async Task GivenNotExistPostId_GetPostById_ReturnNotFound()
        {
            var response = await _client.GetAsync("/api/posts/1");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }


        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
