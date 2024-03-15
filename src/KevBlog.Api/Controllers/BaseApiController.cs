using KevBlog.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
namespace KevBlog.Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
    public record ApiResponse<T>(T Data, bool Success = true, string ErrorMessage = null);
}