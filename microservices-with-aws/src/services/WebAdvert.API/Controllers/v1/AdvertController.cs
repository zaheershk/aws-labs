using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAdvert.Data.Repository;
using WebAdvert.Models;
using WebAdvert.Models.Shared;

namespace WebAdvert.API.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class AdvertController : ControllerBase
    {
        private readonly ILogger<AdvertController> _logger;
        private readonly IAdvertRepository _advertRepository;

        public AdvertController(ILogger<AdvertController> logger, IAdvertRepository advertRepository)
        {
            _logger = logger;
            _advertRepository = advertRepository;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ServiceResponse<IEnumerable<Advert>>), 200)]
        public async Task<IActionResult> All()
        {
            return new JsonResult(
                new ServiceResponse<IEnumerable<Advert>>(
                    await _advertRepository.GetAllAsync()));
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ServiceResponse<Advert>), 404)]
        [ProducesResponseType(typeof(ServiceResponse<Advert>), 200)]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                return new JsonResult(
                    new ServiceResponse<Advert>(
                        await _advertRepository.GetByIdAsync(id)));
            }
            catch (KeyNotFoundException kEx)
            {
                return NotFound(new ServiceResponse<Advert>(null, kEx.Message));
            }
        }

        [HttpPost]
        [Route("create")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ServiceResponse<Advert>), 404)]
        [ProducesResponseType(typeof(ServiceResponse<Advert>), 201)]
        public async Task<IActionResult> CreateAsync(Advert model)
        {
            // create record
            var id = await _advertRepository.AddAsync(model);
            return CreatedAtAction(nameof(GetAsync), new { id }, model);
        }

        [HttpPut]
        [Route("confirm")]
        [ProducesResponseType(typeof(ServiceResponse<Advert>), 404)]
        [ProducesResponseType(typeof(ServiceResponse<Advert>), 200)]
        public async Task<IActionResult> ConfirmAsync(ConfirmAdvert model)
        {
            try
            {
                // update record
                await _advertRepository.ConfirmAsync(model);
                return new OkResult();
            }
            catch (KeyNotFoundException kEx)
            {
                return NotFound(new ServiceResponse<Advert>(null, kEx.Message));
            }
        }
    }
}
