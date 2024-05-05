using HBlog.WebClient.Helpers;
using NUnit.Framework;

namespace HBlog.UnitTests.Helpers
{
    public class QueryHelperTests
    {
        [Test]
        public void ArrayToQueryString_ValidInput()
        {
            var result = QueryHelper.ArrayToQueryString("keyvalue", ["arr1","arr2","arr3"]);

            Assert.That(result, Is.EqualTo("keyvalue=arr1&keyvalue=arr2&keyvalue=arr3"));
        }
    }
}
