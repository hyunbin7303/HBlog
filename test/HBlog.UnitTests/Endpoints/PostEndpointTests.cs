using Moq;
using System.Net;
using System.Text.Json;
using HBlog.Api.Controllers;
using HBlog.Contract.DTOs;
using HBlog.Domain.Common.Params;
using NUnit.Framework;
using static NUnit.Framework.Legacy.CollectionAssert;
using Assert = NUnit.Framework.Assert;
namespace HBlog.UnitTests.Endpoints
{
    public class PostEndpointTests : IDisposable
    {
        private PostAppFactory _factory;
        private HttpClient _client;

        public PostEndpointTests()
        {
            _factory = new PostAppFactory();
            _client = _factory.CreateClient();
        }

        //[Test]
        //public async Task GivenValidPosts_WhenGetPostsCalled_ThenResponsePosts()
        //{
        //    IEnumerable<PostDisplayDto> posts = new List<PostDisplayDto>
        //    {
        //        new() { Id = 1, Title = "PostDisplay#1", Desc = "TestingDesc1", Content = "TestingContent1", UserName="hyunbin7303" },
        //        new() { Id = 2, Title = "PostDisplay#2", Desc = "TestingDesc2", Content = "TestingContent2", UserName="hyunbin7303" },
        //    };
        //    _factory._mockPostService.Setup(x => x.GetPosts(It.IsAny<PostParams>())).ReturnsAsync(posts);
        //    var response = await _client.GetAsync("/api/posts");
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        //    var data = JsonSerializer.Deserialize<ApiResponse<IEnumerable<PostDisplayDto>>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
        //    {
        //        WriteIndented = true,
        //        PropertyNameCaseInsensitive = true
        //    });
        //    IEnumerable<PostDisplayDto> resultPosts = data.Data;

        //    if (resultPosts != null) AllItemsAreNotNull(resultPosts);
        //}


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
