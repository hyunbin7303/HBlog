using KevBlog.Persistence.Aws.S3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KevBlog.Api.Controllers
{

    [Authorize]
    public class StorageController : BaseApiController
    {
        private readonly IAwsStorageService _awsStorageService;

        public StorageController(IAwsStorageService awsStorageService)
        {
            _awsStorageService = awsStorageService;
        }

        //[Authorize]
        //public async Task<IActionResult> UploadFile(string fileName)
        //{
        //    var result = await _awsStorageService.DownloadFileAsync(fileName);
        //    return Ok();
        //}

    }
}
