using KevBlog.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
namespace KevBlog.Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
    }
}