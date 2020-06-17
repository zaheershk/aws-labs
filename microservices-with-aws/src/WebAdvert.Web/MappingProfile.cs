using AutoMapper;
using WebAdvert.Models;
using WebAdvert.Web.Models.AdvertManagement;

namespace WebAdvert.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Advert, AdvertViewModel>().ReverseMap();
            CreateMap<AdvertViewModel, CreateAdvertViewModel>().ReverseMap();
            CreateMap<ConfirmAdvert, ConfirmAdvertViewModel>().ReverseMap();
        }
    }
}
