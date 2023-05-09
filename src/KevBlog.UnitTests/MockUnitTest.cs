using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KevBlog.UnitTests
{
    public class TestClass
    {
        public virtual string GetItem(int i)
        {
            return "";
        }
        public virtual string name { get; set; }
    }
    public class MockUnitTest
    {
        [Fact]
        public void Test()
        {
            Mock<TestClass> mock = new Mock<TestClass>();
            mock.Setup(x => x.GetItem(It.IsAny<int>())).Returns("123");
            mock.Setup(x => x.name).Returns("Kevin");
            TestClass t = mock.Object;
            Assert.Equal("123", t.GetItem(222));
        }
    }
}
