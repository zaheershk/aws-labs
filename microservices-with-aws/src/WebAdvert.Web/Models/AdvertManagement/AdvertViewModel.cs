using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdvert.Web.Models.AdvertManagement
{
    public class AdvertViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string UserName { get; set; }
        public string FilePath { get; set; }
    }
}
