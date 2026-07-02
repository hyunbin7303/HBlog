using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.Infrastructure.Data;
using HBlog.Infrastructure.Repositories;
using HBlog.UnitTests.Mocks.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace HBlog.UnitTests.Repositories
{
    public class UserRepositoryTest : IDisposable
    {
        private readonly IdentityContext _context;
        private readonly IUserRepository _userRepository;
        private readonly List<User> _seededUsers;
        public UserRepositoryTest()
        {
            var dbContextOptions = new DbContextOptionsBuilder<IdentityContext>()
                .UseInMemoryDatabase(databaseName: $"TestingUserRepo_{Guid.NewGuid()}").Options;

            _context = new IdentityContext(dbContextOptions);

            _seededUsers = MockUserRepository.SampleValidUserData(3).ToList();
            _context.Users.AddRange(_seededUsers);
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
            var seeded = _seededUsers[0];

            var user = await _userRepository.GetUserByIdAsync(seeded.Id);

            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(seeded.Id));
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
