using AutoMapper;
using WebAdvert.Data.Models;
using WebAdvert.Models;

namespace WebAdvert.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Advert, AdvertDao>();
        }
    }
}
