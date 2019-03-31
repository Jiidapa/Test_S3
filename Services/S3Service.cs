using System;
using System.Threading.Tasks;
using test_S3.Models;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Amazon.S3.Transfer;
using System.Net;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace test_S3.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _client;


        Chilkat.Http http = new Chilkat.Http();
        Chilkat.Global glob = new Chilkat.Global();


        public S3Service(IAmazonS3 client)
        {
            _client = client;
        }

        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {
            try
            {
                if (await AmazonS3Util.DoesS3BucketExistAsync(_client, bucketName) == false)
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };

                    var response = await _client.PutBucketAsync(putBucketRequest);

                    return new S3Response
                    {
                        Message = response.ResponseMetadata.RequestId,
                        Status = response.HttpStatusCode
                    };
                }
            }
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Status = e.StatusCode,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                return new S3Response
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }

            return new S3Response
            {
                Status = HttpStatusCode.InternalServerError,
                Message = "Something went wrong"
            };
        }

        private const string FilePath = "C:\\Users\\jida\\AWS\\S3Bucket\\jida.txt";
        private const string File = "tests3/" + "TestUploadS3";

        //private const string UploadWithKeyName = "UploadWithKeyName";
        //private const string FileStreamUpload = "FileStreamUpload";
        //private const string AdvancedUpload = "AdvancedUpload";

        public async Task UploadFileAsync(string bucketName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_client);

                // Option 3. Upload data from a type of System.IO.Stream.
                using (var fileToUpload =
                    new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    await fileTransferUtility.UploadAsync(fileToUpload,
                                               bucketName, File);
                }
                Console.WriteLine("Upload 1 completed");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

        public  void GetObjectFromS3Async(string bucketName)
        {
            bool s = glob.UnlockBundle("Anything for 30-day trial");
            if (s != true)
            {
                Console.WriteLine(glob.LastErrorText);
                return;
            }

            Console.WriteLine(glob.LastErrorText);

     
            const string keyName = "manual/G-able Community.docx";
            const string fileName = "คู่มือการใช้งานโปรแกรมจัดการไฟล์.docx";
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            string localDownload = Path.Combine(pathDownload, fileName);


            try
            {
                http.AwsAccessKey = "AKIAIF32XCLYPMT42QAA";
                http.AwsSecretKey = "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";
                http.S3Ssl = true;
                http.AwsRegion = "us-west-2";
                http.AwsEndpoint = "s3-us-west-2.amazonaws.com";

                bool success = http.S3_DownloadFile(bucketName, keyName, localDownload);
                

                if (success != true)
                {
                    Debug.WriteLine(http.LastErrorText);
                }
                else
                {
                    Debug.WriteLine("File downloaded.");
                }

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }

            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }


        }

        public string GeneratePreSignedURL(string bucketName)
        {
           
            string keyName = "manual/G-able Community.docx";
            string urlString = "";
            try
            {
                GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    Expires = DateTime.Now.AddMinutes(5)
                };
                urlString = _client.GetPreSignedURL(request1);
                return urlString;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            return urlString;
        }


    }
}
