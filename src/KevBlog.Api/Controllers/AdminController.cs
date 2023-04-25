using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KevBlog.Api.Controllers
{
    public class AdminController : BaseApiController
    {
        public AdminController()
        {

        }

        [Authorize(Policy ="RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public ActionResult GetUsersWithRoles()
        {
            return Ok("Only Admins can see this.");
        }

        [Authorize(Policy ="ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators can see this.");
        }
    }
}
