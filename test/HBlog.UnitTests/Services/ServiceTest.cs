using HBlog.TestUtilities;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;

namespace HBlog.UnitTests.Services
{
    [TestFixture]
    class ServiceTest : TestBase
    {
        protected ServiceTest()
        {
            var webHostEnv = new Mock<IWebHostEnvironment>();
            webHostEnv.Setup(x => x.ContentRootPath)
                .Returns(System.Reflection.Assembly.GetExecutingAssembly().Location);
            webHostEnv.Setup(x => x.WebRootPath).Returns(Directory.GetCurrentDirectory());

        }
    }
}
