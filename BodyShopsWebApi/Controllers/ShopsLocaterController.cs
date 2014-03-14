using BodyShopsWebApi.Models;
using BodyShopsWebApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BodyShopsWebApi.Controllers
{
    public class ShopsLocaterController : ApiController
    {
        private static readonly IShopRepository repository = new ShopRepository();             

        [HttpGet]
        public IList<Shop> Get(string providerName, double latitude, double longitude, int range)
        {
            var providerId = repository.GetCliamProvider(providerName);
            if (providerId == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(String.Format("Provider Name ' {0} ' was not found.", providerName)),
                    ReasonPhrase = String.Format("Provider Name ' {0} ' was not found.", providerName)
                });
            }
            var shops = repository.GetProviderBodyShopsInRange(providerId.Value, new GeoLocation(latitude, longitude), range);
            if (shops.Count == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(String.Format("DRP Shops doesn't exist in specified {0} K.M range.", range)),
                    ReasonPhrase = String.Format("DRP Shops doesn't exist in specified {0} K.M range.", range)
                });
            }
            else
            {
                return shops;
                //var response = Request.CreateResponse(HttpStatusCode.OK, item);
                //response.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(300));
                //return response;
            }
        }        
    }
}
