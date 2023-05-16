﻿using KevBlog.Application.DTOs;
using KevBlog.Application.Services;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.UnitTests.Repositories;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KevBlog.UnitTests.Services
{
    public class PostServiceTest : ServiceTest
    {
        private IPostService _postService;
        private Mock<IPostRepository> _postRepositoryMock = new();
        private Mock<IUserRepository> _userRepositoryMock = new();

        public PostServiceTest()
        {
            _postService = new PostService(_mapper, _postRepositoryMock.Object, _userRepositoryMock.Object);
        }
        [Fact]
        public async Task GetPosts_ExistingInRepo_ReturnSuccess()
        {
            int howMany = 5;
            var testObject = MockIPostRepository.GenerateData(howMany);
            _postRepositoryMock.Setup(x => x.GetPostsAsync().Result).Returns(testObject);

            var posts = await _postService.GetPosts();

            Assert.NotNull(posts);
            Assert.Equal(howMany, posts.Count());
        }


        [Fact]
        public async Task GetPostById_ExistingPostId_ReturnPostSuccessfully()
        {
            int postId = 1;
            var testObject = MockIPostRepository.GenerateData(5);
            _postRepositoryMock.Setup(x => x.GetPostById(postId)).ReturnsAsync(testObject.Where(x=> x.Id == postId).FirstOrDefault());

            var postDetails = await _postService.GetByIdAsync(1);

            Assert.NotNull(postDetails);
            Assert.Equal(true, postDetails.IsSuccess);
            Assert.IsType<PostDisplayDetailsDto>(postDetails.Value);
            _postRepositoryMock.Verify(x => x.GetPostById(postId));
        }

        [Fact]
        public async Task CreatePost_PassValidDto_IsSuccessTrue()
        {
            int postId = 1;
            var testObject = MockIPostRepository.GenerateData(5)[0];
            string userName = "kevin0";
            PostCreateDto postCreateDto = new PostCreateDto()
            {
                Title = "New Post Create",
                Desc = "New Post Desc",
                LinkForPost = "https://github.com/hyunbin7303",
                Type = "Programming"
            };
            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(userName)).ReturnsAsync(GetSampleUser());
            _postRepositoryMock.Setup(x => x.GetPostById(It.Is<int>(i => i == postId))).ReturnsAsync(() => testObject);

            // Action 
            var result = await _postService.CreatePost(userName, postCreateDto);

            Assert.Equal(true, result.IsSuccess);
            _userRepositoryMock.Verify(x => x.GetUserByUsernameAsync("kevin0"), Times.Once);
            _postRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task CreatePost_PostTitleNull_IsSuccessFalse()
        {
            var testObject = MockIPostRepository.GenerateData(5)[0];
            string userName = "kevin0";
            int postId = 1;
            PostCreateDto postCreateDto = new PostCreateDto()
            {
                Title = null,
                Desc = "New Post Desc",
                LinkForPost = "https://github.com/hyunbin7303",
                Type = "Programming"
            };
            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(userName)).ReturnsAsync(GetSampleUser());
            _postRepositoryMock.Setup(x => x.GetPostById(It.Is<int>(i => i == postId))).ReturnsAsync(() => testObject);

            var result = await _postService.CreatePost(userName, postCreateDto);

            Assert.Equal(false, result.IsSuccess);
            Assert.Equal("Title cannot be empty.", result.Message);
            _userRepositoryMock.Verify(x => x.GetUserByUsernameAsync("kevin0"), Times.Never);
            _postRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Post>()), Times.Never);
        }



        [Fact]
        public async Task UpdatePost_Checking_Success()
        {
            var testObject = MockIPostRepository.GenerateData(5)[0];
            int postId = testObject.Id;
            PostUpdateDto postUpdateDto = new PostUpdateDto()
            {
                Id = 1,
                Title = "Post Update DTO Update",
                Desc = "Post Update new Desc",
                Content = "Content new info",
                LinkForPost = "",
                Type = "Programming"
            };
            _postRepositoryMock.Setup(x => x.GetPostById(postId)).ReturnsAsync(testObject);

            var result = await _postService.UpdatePost(postUpdateDto); 

            Assert.Equal(true, result.IsSuccess);
            _postRepositoryMock.Verify(x => x.GetPostById(postId), Times.Once);
            _postRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Post>()), Times.Once);

        }
    }
}
