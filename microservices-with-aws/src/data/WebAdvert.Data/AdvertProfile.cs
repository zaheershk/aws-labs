using AutoMapper;
using WebAdvert.Data.Models;
using WebAdvert.Models;

namespace WebAdvert.API.Models
{
    public class AdvertProfile : Profile
    {
        public AdvertProfile()
        {
            CreateMap<Advert, AdvertDao>();
        }
    }
}
