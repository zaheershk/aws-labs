using WebAdvert.Models;

namespace WebAdvert.Web.Models.AdvertManagement
{
    public class ConfirmAdvertViewModel
    {
        public string Id { get; set; }
        public string FilePath { get; set; }
        public AdvertStatus Status { get; set; }
    }
}
