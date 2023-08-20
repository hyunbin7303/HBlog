using KevBlog.Application.Services;
using KevBlog.Domain.Repositories;
using KevBlog.Persistence.Aws.S3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection.Metadata.Ecma335;

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

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(List<IFormFile> formFiles, CancellationToken token)
        {
            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in formFiles)
            {
                using (var ms = new MemoryStream())
                {
                    await postedFile.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    var result = await _awsStorageService.UploadFileAsync(ms, "bucket","KevinBucket", postedFile.FileName);
                }
            }
            return Ok();
        }

        [HttpPost("CreateBucket")]
        public async Task<IActionResult> CreateBucket(string bucketName)
        {

            return Ok();
        }

    }
}
