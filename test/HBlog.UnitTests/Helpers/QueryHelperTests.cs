using HBlog.WebClient.Helpers;
using NUnit.Framework;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net.Http;

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
        [Test]
        public void BuildUrlWithQueryStringUsingUriBuilder_ValidInput()
        {

            var query = new Dictionary<string, string>
            {
                { "categoryId",  "1" },
            };

            var baseurl = "https://localhost:5001/api/";
            var url = QueryHelper.BuildUrlWithQueryStringUsingUriBuilder($"{baseurl}posts", query);

            Assert.That(url, Is.EqualTo("https://localhost:5001/api/posts?categoryId=1"));
        }
    }
}
