using BodyShopsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyShopsApp.Repository
{
    interface IShopRepository
    {
        IEnumerable<BodyShop> GetAll();
        BodyShop Get(int id);
        bool Add(BodyShop item);
        bool Remove(int id);
        bool Update(int shopId, BodyShop item);
    }
}
