using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_S3.Models;

namespace test_S3.Services
{
    public interface IS3Service
    {
        Task<S3Response> CreateBucketAsync(string bucketName);
        Task UploadFileAsync(string bucketName);
        void GetObjectFromS3Async(string bucketName);
        string GeneratePreSignedURL(string bucketName);
    }
}
