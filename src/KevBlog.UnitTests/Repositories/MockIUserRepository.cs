using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Moq;

namespace KevBlog.UnitTests.Repositories
{
    internal class MockIUserRepository
    {
        public static Mock<IUserRepository> GetMock()
        {
            var mock = new Mock<IUserRepository>();

            IEnumerable<User> userList = SampleValidUserData(3);
            mock.Setup(m => m.GetUsersAsync().Result).Returns(() => userList);
            return mock;
        }
        public static IEnumerable<User> SampleValidUserData(int howMany)
        {
            List<User> users = new();
            for (int i = 0; i < howMany; i++)
            {
                User user = new()
                {
                    Id = i,
                    UserName = "kevin" + i,
                    KnownAs = "knownas" + i,
                    Gender = "Male",
                    City = "Kitchener",
                    DateOfBirth = new DateTime(1993, 7, 3),
                    Email = "hyunbin7303@gmail.com",
                };
                users.Add(user);
            }
            return users;
        }
    }
}
