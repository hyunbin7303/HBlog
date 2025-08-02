using HBlog.Domain.Entities;
using HBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HBlog.Api.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _dbContext;
        public BuggyController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Authorize]
        [HttpGet("buggy/auth")]
        public ActionResult<string> GetSecret()
        {
            return "Secret text";
        }

        [HttpGet("buggy/not-found")]
        public ActionResult<User> GetNotFound()
        {
            var thing = _dbContext.Users.Find(-1);
            if (thing == null) return NotFound();
            return thing;
        }

        [HttpGet("buggy/server-error")]
        public ActionResult<string> GetServerError()
        {
            return _dbContext.Users.Find(-1)?.ToString();
        }
        [HttpGet("buggy/bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request.");
        }
    }
}