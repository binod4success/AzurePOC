using BodyShopsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyShopsApp.Repository
{
    interface IShopLocaterRepository
    {
        IList<BodyShop> GetProviderBodyShopsInRange(string providerName, GeoLocation currentLoc, int range);
    }
}
