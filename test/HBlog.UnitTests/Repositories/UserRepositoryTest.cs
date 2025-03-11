using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.Infrastructure.Data;
using HBlog.Infrastructure.Repositories;
using HBlog.UnitTests.Mocks.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace HBlog.UnitTests.Repositories
{
    public class UserRepositoryTest : IDisposable
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        public UserRepositoryTest()
        {
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestingUserRepo").Options;

            _context = new DataContext(dbContextOptions);

            IEnumerable<User> userList = MockUserRepository.SampleValidUserData(3);
            _context.Users.AddRange(userList);
            _context.SaveChanges();
            _userRepository = new UserRepository(_context);
        }

        [Test]
        public async Task WhenGetUser_ThenReturnUsers()
        {
            var users = await _userRepository.GetUsersAsync();

            Assert.That(users, Is.Not.Null);
        }

        [Test]
        public async Task GivenExistingUserName_WhenGetUserByUsername_ThenReturnUser()
        {
            var user = await _userRepository.GetUserByUsernameAsync("kevin1");

            Assert.That(user, Is.Not.Null);
            Assert.That(user.UserName, Is.EqualTo("kevin1"));
        }


        [Test]
        public async Task GivenNotExistingUserName_WhenGetUserByUsername_TheReturnNull()
        {
            var user = await _userRepository.GetUserByUsernameAsync("nouser");

            Assert.That(user, Is.Null);
        }

        [Test]
        public async Task GivenValidUserId_WhenGetUserById_ThenReturnUser()
        {
            var userId = Guid.CreateVersion7();

            var user = await _userRepository.GetUserByIdAsync(userId);

            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(userId));
        }

        [Test]
        public async Task GivenInvalidUserId_WhenGetUserById_ThenReturnNull()
        {
            var user = await _userRepository.GetUserByIdAsync(Guid.CreateVersion7());

            Assert.That(user, Is.Null);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
