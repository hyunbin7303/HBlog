using KevBlog.Domain.Entities;
using KevBlog.Domain.Params;
using Moq;

namespace KevBlog.UnitTests.Services
{


    public class ServiceTest : TestBase
    {
        public ServiceTest()
        {

        }
        public User GetSampleUser()
        {
            return new User() { Id = 1, Email= "hyunbin7303@gmail.com", UserName = "hyunbin7303", City = "Kitchener", Country = "Canada"};
        }
    }
}
