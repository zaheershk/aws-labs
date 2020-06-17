using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebAdvert.Models;
using WebAdvert.Models.Shared;

namespace Files.API.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IConfiguration _configuration;

        public FilesController(ILogger<FilesController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ServiceResponse<string>), 404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            PutObjectResponse response;

            try
            {
                // upload file
                using Stream fs = file.OpenReadStream();
                using var client = new AmazonS3Client();

                if (fs.Length > 0)
                    if (fs.CanSeek)
                        fs.Seek(0, SeekOrigin.Begin);

                var request = new PutObjectRequest
                {
                    AutoCloseStream = true,
                    BucketName = _configuration.GetValue<string>("AWS:ImageBucketName"),
                    InputStream = fs,
                    Key = file.FileName
                };

                response = await client.PutObjectAsync(request).ConfigureAwait(false);
                if (response.HttpStatusCode != HttpStatusCode.OK)
                {
                    return (StatusCode(500,
                        new ServiceResponse<Advert>(null, $"File upload failed. S3 api responded with a {response.HttpStatusCode} status-code.")));
                }
            }
            catch (AmazonS3Exception ae)
            {
                return (StatusCode(500,
                    new ServiceResponse<Advert>(null, ae.Message)));
            }

            return new OkResult();
        }
    }
}
