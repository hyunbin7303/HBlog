﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper.Execution;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Common;
using HBlog.Domain.Entities;
using HBlog.Domain.Params;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
        public async Task GivenExistingUser_WhenGetUserByUserName_ThenReturnUser()
        {
            var searchUser = new MemberDto()
            {
                Id = 1,
                UserName = "test",
                Introduction = "Testing",
                KnownAs = "Kevin",
                Gender = "male",
                Age = 30,
                Created = DateTime.Now,
                LastActive = DateTime.Now,
                PhotoUrl = "checkout@source.com/1",
                Posts = new()
            };
            ServiceResult<MemberDto> result = new()
            {
                IsSuccess = true,
                Message = "hi",
                Value = searchUser
            };
            _factory._mockUserService.Setup(o => o.GetMembersByUsernameAsync(It.IsAny<string>())).ReturnsAsync(result);
            _factory._mockUserRepository.Setup(o => o.GetUserByIdAsync(1)).ReturnsAsync(new User() { Id = 1, UserName = "test" });
            var response = await _client.GetAsync("/api/users/test");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var data = JsonSerializer.Deserialize<ServiceResult<MemberDto>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
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


        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}