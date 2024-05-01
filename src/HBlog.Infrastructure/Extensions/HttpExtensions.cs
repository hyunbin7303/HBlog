using System.Text.Json;
using HBlog.Domain.Common;
using Microsoft.AspNetCore.Http;

namespace HBlog.Infrastructure.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
        {
            var jsonOptions= new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(header,jsonOptions));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
        
    }
}