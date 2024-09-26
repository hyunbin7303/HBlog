using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HBlog.Domain.Entities;
using HBlog.Infrastructure.Data;
using HBlog.Infrastructure.Repositories;
using HBlog.UnitTests.Mocks.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace HBlog.UnitTests.Repositories
{
    public class UserRepositoryTest : IDisposable
    {
        private readonly DbContextOptions<DataContext> dbContextOptions;
        private readonly DataContext _context;

        public UserRepositoryTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Testing").Options;

            _context = new DataContext(this.dbContextOptions);

            IEnumerable<User> userList = MockUserRepository.SampleValidUserData(3);
            _context.Users.AddRange(userList);

            _context.SaveChanges();
        }

        [Test]
        public async Task WhenGetUser_ThenReturnUsers()
        {
            var repo = new UserRepository(_context);

            var users = await repo.GetUsersAsync();

            Assert.That(users, Is.Not.Null);
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
