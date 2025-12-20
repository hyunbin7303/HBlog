//using HBlog.Domain.Entities;
//using HBlog.Domain.Repositories;
//using HBlog.Infrastructure.Repositories;
//using HBlog.IntegrationTests.Base;
//using NUnit.Framework;
//using Assert = NUnit.Framework.Assert;

//namespace HBlog.IntegrationTests.Repositories
//{
//    [TestFixture]
//    public class PostRepositoryIntegrationTests : DatabaseIntegrationTestBase
//    {
//        private IPostRepository _postRepository;

//        [SetUp]
//        public async Task PostRepositorySetUp()
//        {
//            await SetUp();
//            _postRepository = new PostRepository(_dbContext);
//        }

//        [Test]
//        public async Task CreatePost_ShouldSavePostToDatabase()
//        {
//            // Arrange
//            var testUser = await GetTestUserAsync();
//            var testCategory = await GetTestCategoryAsync();

//            var post = new Post
//            {
//                Title = "Test Post",
//                Content = "This is a test post content",
//                Desc = "Test description",
//                UserId = testUser.Id,
//                User = testUser,
//                CategoryId = testCategory.Id,
//                Category = testCategory,
//                Created = DateTime.UtcNow
//            };

//            _postRepository.Add(post);
//            await _dbContext.SaveChangesAsync();

//            var savedPost = await _postRepository.GetPostDetails(post.Id);
//            Assert.That(savedPost, Is.Not.Null);
//            Assert.That(savedPost.Title, Is.EqualTo("Test Post"));
//            Assert.That(savedPost.UserId, Is.EqualTo(testUser.Id));
//        }

//        [Test]
//        public async Task GetPostsAsync_ShouldReturnAllPosts()
//        {
//            // Arrange
//            var testUser = await GetTestUserAsync();
//            var testCategory = await GetTestCategoryAsync();

//            var posts = new List<Post>
//            {
//                new()
//                {
//                    Title = "Post 1",
//                    Content = "Content 1",
//                    Desc = "Description 1",
//                    UserId = testUser.Id,
//                    User = testUser,
//                    CategoryId = testCategory.Id,
//                    Created = DateTime.UtcNow
//                },
//                new()
//                {
//                    Title = "Post 2",
//                    Content = "Content 2",
//                    Desc = "Description 2",
//                    UserId = testUser.Id,
//                    User = testUser,
//                    CategoryId = testCategory.Id,
//                    Created = DateTime.UtcNow
//                }
//            };

//            foreach (var post in posts)
//            {
//                _postRepository.Add(post);
//            }
//            await _dbContext.SaveChangesAsync();

//            var retrievedPosts = await _postRepository.GetPostsAsync();

//            Assert.That(retrievedPosts.Count(), Is.EqualTo(2));
//            Assert.That(retrievedPosts.Any(p => p.Title == "Post 1"), Is.True);
//            Assert.That(retrievedPosts.Any(p => p.Title == "Post 2"), Is.True);
//        }

//        [Test]
//        public async Task GetPostDetails_ShouldReturnPostWithUserAndTags()
//        {
//            // Arrange
//            var testUser = await GetTestUserAsync();
//            var testCategory = await GetTestCategoryAsync();
//            var testTag = await GetTestTagAsync();

//            var post = new Post
//            {
//                Title = "Detailed Post",
//                Content = "Content with details",
//                Desc = "Detailed description",
//                UserId = testUser.Id,
//                User = testUser,
//                CategoryId = testCategory.Id,
//                Created = DateTime.UtcNow
//            };

//            _postRepository.Add(post);
//            var numCheck = await _dbContext.SaveChangesAsync();

//            // Add tag relationship
//            var postTag = new PostTags { PostId = numCheck, TagId = testTag.Id };
//            _dbContext.PostTags.Add(postTag);
//            await _dbContext.SaveChangesAsync();

//            // Act
//            var detailedPost = await _postRepository.GetPostDetails(numCheck);

//            // Assert
//            Assert.That(detailedPost, Is.Not.Null);
//            Assert.That(detailedPost.User, Is.Not.Null);
//            Assert.That(detailedPost.User.UserName, Is.EqualTo("testuser1"));
//            Assert.That(detailedPost.Tags, Is.Not.Null);
//            Assert.That(detailedPost.Tags.Any(t => t.Name == "C#"), Is.True);
//        }

//        [Test]
//        public async Task GetPostsTitleContainsAsync_ShouldReturnMatchingPosts()
//        {
//            // Arrange
//            var testUser = await GetTestUserAsync();
//            var testCategory = await GetTestCategoryAsync();

//            var posts = new List<Post>
//            {
//                new()
//                {
//                    Title = "Blazor Tutorial",
//                    Content = "Learn Blazor",
//                    Desc = "Tutorial description",
//                    UserId = testUser.Id,
//                    CategoryId = testCategory.Id,
//                    Created = DateTime.UtcNow
//                },
//                new()
//                {
//                    Title = "ASP.NET Core Guide",
//                    Content = "Learn ASP.NET",
//                    Desc = "Guide description",
//                    UserId = testUser.Id,
//                    CategoryId = testCategory.Id,
//                    Created = DateTime.UtcNow
//                }
//            };

//            foreach (var post in posts)
//            {
//                _postRepository.Add(post);
//            }
//            await _dbContext.SaveChangesAsync();

//            // Act
//            var blazorPosts = await _postRepository.GetPostsTitleContainsAsync("blazor");
//            var aspPosts = await _postRepository.GetPostsTitleContainsAsync("asp");

//            // Assert
//            Assert.That(blazorPosts.Count(), Is.EqualTo(1));
//            Assert.That(blazorPosts.First().Title, Is.EqualTo("Blazor Tutorial"));

//            Assert.That(aspPosts.Count(), Is.EqualTo(1));
//            Assert.That(aspPosts.First().Title, Is.EqualTo("ASP.NET Core Guide"));
//        }

//        [Test]
//        public async Task GetPostsByUserName_ShouldReturnUserPosts()
//        {
//            // Arrange
//            var testUser1 = await GetTestUserAsync("testuser1");
//            var testUser2 = await GetTestUserAsync("testuser2");
//            var testCategory = await GetTestCategoryAsync();

//            var user1Posts = new List<Post>
//            {
//                new()
//                {
//                    Title = "User1 Post 1",
//                    Content = "Content by user1",
//                    Desc = "Description 1",
//                    UserId = testUser1.Id,
//                    User = testUser1,
//                    CategoryId = testCategory.Id,
//                    Created = DateTime.UtcNow
//                },
//                new()
//                {
//                    Title = "User1 Post 2",
//                    Content = "Another content by user1",
//                    Desc = "Description 2",
//                    UserId = testUser1.Id,
//                    User = testUser1,
//                    CategoryId = testCategory.Id,
//                    Created = DateTime.UtcNow
//                }
//            };

//            var user2Post = new Post
//            {
//                Title = "User2 Post",
//                Content = "Content by user2",
//                Desc = "User2 description",
//                UserId = testUser2.Id,
//                User = testUser2,
//                CategoryId = testCategory.Id,
//                Created = DateTime.UtcNow
//            };

//            foreach (var post in user1Posts)
//            {
//                _postRepository.Add(post);
//            }
//            _postRepository.Add(user2Post);
//            await _dbContext.SaveChangesAsync();

//            // Act
//            var user1PostsResult = await _postRepository.GetPostsByUserName("testuser1");

//            // Assert
//            Assert.That(user1PostsResult.Count(), Is.EqualTo(2));
//            Assert.That(user1PostsResult.All(p => p.User.UserName == "testuser1"), Is.True);
//        }

//        [Test]
//        public async Task UpdateAsync_ShouldUpdatePostInDatabase()
//        {
//            // Arrange
//            var testUser = await GetTestUserAsync();
//            var testCategory = await GetTestCategoryAsync();

//            var post = new Post
//            {
//                Title = "Original Title",
//                Content = "Original Content",
//                Desc = "Original Description",
//                UserId = testUser.Id,
//                CategoryId = testCategory.Id,
//                Created = DateTime.UtcNow
//            };

//            _postRepository.Add(post);
//            await _dbContext.SaveChangesAsync();

//            //// Act
//            //createdPost.Title = "Updated Title";
//            //createdPost.Content = "Updated Content";
//            //await _postRepository.UpdateAsync(createdPost);

//            //// Assert
//            //var updatedPost = await _postRepository.GetById(createdPost.Id);
//            //Assert.That(updatedPost.Title, Is.EqualTo("Updated Title"));
//            //Assert.That(updatedPost.Content, Is.EqualTo("Updated Content"));
//        }

//        [Test]
//        public async Task GetPostsAsync_WithLimitAndOffset_ShouldReturnPagedResults()
//        {
//            // Arrange
//            var testUser = await GetTestUserAsync();
//            var testCategory = await GetTestCategoryAsync();

//            // Create 5 posts
//            for (int i = 1; i <= 5; i++)
//            {
//                var post = new Post
//                {
//                    Title = $"Post {i}",
//                    Content = $"Content {i}",
//                    Desc = $"Description {i}",
//                    UserId = testUser.Id,
//                    CategoryId = testCategory.Id,
//                    Created = DateTime.UtcNow.AddMinutes(-i) // Different created times
//                };
//                _postRepository.Add(post);
//            }
//            await _dbContext.SaveChangesAsync();

//            // Act
//            var firstPage = await _postRepository.GetPostsAsync(limit: 2, offset: 0);
//            var secondPage = await _postRepository.GetPostsAsync(limit: 2, offset: 2);

//            // Assert
//            Assert.That(firstPage.Count(), Is.EqualTo(2));
//            Assert.That(secondPage.Count(), Is.EqualTo(2));

//            // Should be ordered by Created descending (most recent first)
//            var firstPageList = firstPage.ToList();
//            Assert.That(firstPageList[0].Title, Is.EqualTo("Post 1"));
//            Assert.That(firstPageList[1].Title, Is.EqualTo("Post 2"));
//        }
//    }
//}