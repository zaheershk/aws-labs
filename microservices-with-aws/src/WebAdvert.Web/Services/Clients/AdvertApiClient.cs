using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebAdvert.Models;
using WebAdvert.Models.Shared;
using WebAdvert.Web.Models.AdvertManagement;
using System.Net;

namespace WebAdvert.Web.Services.Clients
{
    public class AdvertApiClient : BaseClient
    {
        private readonly string _baseAddress;
        private readonly IMapper _mapper;

        public AdvertApiClient(IConfiguration configuration, IMapper mapper, HttpClient client)
            : base(client)
        {
            _mapper = mapper;
            _baseAddress = configuration.GetSection("AdvertApi").GetValue<string>("BaseUrl");
        }

        public async Task<List<AdvertViewModel>> GetAllAsync()
        {
            var response = await GetAsync<ServiceResponse<IEnumerable<Advert>>>($"{_baseAddress}/all");
            return response.Object.Select(x => _mapper.Map<AdvertViewModel>(x)).ToList();
        }

        public async Task<AdvertViewModel> GetAsync(string advertId)
        {
            var response = await GetAsync<ServiceResponse<Advert>>($"{_baseAddress}/{advertId}");
            return _mapper.Map<AdvertViewModel>(response.Object);
        }

        public async Task<ServiceResponse<Advert>> CreateAsync(AdvertViewModel model)
        {
            var apiModel = _mapper.Map<Advert>(model);
            var response = await PostAsync<ServiceResponse<Advert>, Advert>($"{_baseAddress}/create", apiModel);
            return response;
        }

        public async Task<bool> ConfirmAsync(ConfirmAdvertViewModel model)
        {
            var apiModel = _mapper.Map<ConfirmAdvert>(model);
            var response = await PutAsync<HttpResponseMessage, ConfirmAdvert>($"{_baseAddress}/confirm", apiModel);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
