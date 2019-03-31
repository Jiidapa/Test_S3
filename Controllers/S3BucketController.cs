using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test_S3.Services;

namespace test_S3.Controllers
{
    [Produces("application/json")]
    [Route("api/S3Bucket")]
    [ApiController]
    public class S3BucketController : ControllerBase
    {
        private readonly IS3Service _service;

        public S3BucketController (IS3Service service)
        {
            _service = service;
        }

        [HttpPost("{bucketName}")]
        public async Task<IActionResult> CreateBucket([FromRoute] string bucketName)
        {
            var response = await _service.CreateBucketAsync(bucketName);
            return Ok(response);
        }

        [HttpPost]
        [Route("AddFile/{bucketName}")]
        public async Task<IActionResult> AddFile([FromRoute] string bucketName)
        {
            await _service.UploadFileAsync(bucketName);

            return Ok();
        }

        [HttpPost]
        [Route("GetFile/{bucketName}")]
        public  IActionResult GetObjectFromS3Async([FromRoute] string bucketName)
        {
             _service.GetObjectFromS3Async(bucketName);
            return Ok();
      
        }

        [HttpGet("generateurl/{bucketName}")]
        public ActionResult GeneratePreSignedURL(string bucketName)
        {
            try
            {
                string url = "";
                if (bucketName != null)
                {
                   url = _service.GeneratePreSignedURL(bucketName); 
                }
                return Ok(url);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}