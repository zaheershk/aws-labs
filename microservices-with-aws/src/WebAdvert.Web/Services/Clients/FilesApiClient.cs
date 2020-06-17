using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebAdvert.Models;
using WebAdvert.Models.Shared;
using WebAdvert.Web.Models.AdvertManagement;

namespace WebAdvert.Web.Services.Clients
{
    public class FilesApiClient : BaseClient
    {
        private readonly string _baseAddress;
        private readonly IMapper _mapper;

        public FilesApiClient(IConfiguration configuration, IMapper mapper, HttpClient client)
            : base(client)
        {
            _mapper = mapper;
            _baseAddress = configuration.GetSection("FilesApi").GetValue<string>("BaseUrl");
        }

        public async Task<bool> UploadAsync(IFormFile file)
        {
            using var multipartContent = new MultipartFormDataContent
            {
                { new StreamContent(file.OpenReadStream()), "imagefile", file.FileName }
            };

            var response = await _client.PostAsync($"{_baseAddress}/upload", multipartContent);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
