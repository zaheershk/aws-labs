using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAdvert.Models;

namespace WebAdvert.Data.Repository
{
    public interface IAdvertRepository
    {
        Task<IEnumerable<Advert>> GetAllAsync();
        Task<Advert> GetByIdAsync(string id);
        Task<string> AddAsync(Advert model);
        Task ConfirmAsync(ConfirmAdvert model);
        Task<bool> CheckHealthAsync();
    }
}
