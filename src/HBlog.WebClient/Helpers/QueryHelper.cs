using System.Text;

namespace HBlog.WebClient.Helpers
{
    public class QueryHelper
    {
        public static string ArrayToQueryString(string key, string[] values)
        {
            StringBuilder queryString = new StringBuilder();

            // Append each value to the query string
            foreach (string value in values)
            {
                if (queryString.Length > 0)
                {
                    queryString.Append("&");
                }
                queryString.Append(Uri.EscapeDataString(key));
                queryString.Append("=");
                queryString.Append(Uri.EscapeDataString(value));
            }

            return queryString.ToString();
        }

        public static string BuildUrlWithQueryStringUsingUriBuilder(string basePath, Dictionary<string, string> queryParams)
        {
            var uriBuilder = new UriBuilder(basePath)
            {
                Query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"))
            };
            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}
