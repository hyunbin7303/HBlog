using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace HBlog.UnitTests.Endpoints
{
    public class UserEndpointTests : IDisposable
    {
        private UserAppFactory _factory;
        private HttpClient _client;

        public UserEndpointTests()
        {
            _factory = new UserAppFactory();
            _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        }

        [Test]
        public async Task GivenNotExistingUser_WhenGetUserByUsername_ThenNotFound()
        {
            _factory._mockUserRepository.Setup(o => o.GetUserByIdAsync(Guid.CreateVersion7()))
                .ReturnsAsync(new User { Id = Guid.CreateVersion7(), UserName = "test" });

            var response = await _client.GetAsync("/api/users/random");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }


        [Test]
        public async Task GivenExistingUser_WhenGetUserByUserName_ThenHttpStatusOkWithUser()
        {
            var searchUser = new UserDto()
            {
                Id = Guid.CreateVersion7(),
                UserName = "test",
                Introduction = "Testing",
                KnownAs = "Kevin",
                Age = 30,
                Created = DateTime.Now,
                LastActive = DateTime.Now,
                PhotoUrl = "checkout@source.com/1",
            };
            ServiceResult<UserDto> result = new()
            {
                IsSuccess = true,
                Message = "hi",
                Value = searchUser
            };
            _factory._mockUserService.Setup(o => o.GetMembersByUsernameAsync(It.IsAny<string>())).ReturnsAsync(result);
            _factory._mockUserRepository.Setup(o => o.GetUserByIdAsync(searchUser.Id))
                .ReturnsAsync(new User { Id = searchUser.Id, UserName = "test" });
            

            var response = await _client.GetAsync("/api/users/test");


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var data = JsonSerializer.Deserialize<ServiceResult<UserDto>>(await response.Content.ReadAsStringAsync()
                , new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            });
            var member = data.Value;
            Assert.That(member.Id, Is.EqualTo(searchUser.Id));
            Assert.That(member.UserName, Is.EqualTo(searchUser.UserName));
            Assert.That(member.Age, Is.EqualTo(searchUser.Age));
            Assert.That(member.KnownAs, Is.EqualTo(searchUser.KnownAs));
        }



        [Test]
        public async Task Given_WhenUpdateUser_ThenSuccess()
        {
            var response = await _client.GetAsync("/api/users/random");

        }


        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
