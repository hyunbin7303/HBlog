using KevBlog.Application.Services;
using KevBlog.Infrastructure.Extensions;
using KevBlog.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KevBlog.Api.Controllers
{

    [Authorize]
    public class StorageController : BaseApiController
    {
        private readonly IAwsStorageService _awsStorageService;
        private readonly IUserService _userService;
        public StorageController(IAwsStorageService awsStorageService, IUserService userService)
        {
            _awsStorageService = awsStorageService;
            _userService = userService;
        }

        // TODO Implementation test cases. 
        [HttpPost("storage/Bucket/{bucketName}/UploadFile")]
        public async Task<IActionResult> UploadFile(List<IFormFile> formFiles, string bucketName, CancellationToken token)
        {
            var isAuthorized = await _awsStorageService.IsAuthorized(bucketName, User.GetUserId());
            if(!isAuthorized)
                return Unauthorized($"User is not authorized to access to this bucket: {bucketName}");
            
            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in formFiles)
            {
                using (var ms = new MemoryStream())
                {
                    await postedFile.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    var result = await _awsStorageService.UploadFileAsync(ms,"KevinBucket", postedFile.FileName);
                }
            }
            return Ok();
        }

        [HttpGet("storage/GetFile/{bucketName}")]
        public async Task<IActionResult> GetFile(string bucketName, CancellationToken token)
        {

            return Ok();
        }

        [HttpPost("storage/Bucket")]
        public async Task<IActionResult> CreateBucket(string bucketName)
        {
            var result = await _awsStorageService.CreateBucketAsync(bucketName, User.GetUserId());
            if(result) return Ok();
            return BadRequest("Creating bucket Failure");
        }

    }
}
