using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BodyShopsWebApi.Models
{
    public class Shop
    {
        public Shop()
        {
            ShopGeoLocation = new GeoLocation();
        }

        public int Id { get; set; }

        public int ProviderId { get; set; }

        public string ProviderName { get; set; }

        public string Name { get; set; }
        
        public string AddressLine1 { get; set; }
        
        public string AddressLine2 { get; set; }
        
        public string State { get; set; }
        
        public string City { get; set; }
        
        public string Pin { get; set; }
        
        public string Country { get; set; }
        
        public string Contact { get; set; }

        public GeoLocation ShopGeoLocation { get; set; }
    }
}