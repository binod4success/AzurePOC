using BodyShopsApp.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;


namespace BodyShopsApp.Repository
{
    public class ShopLocaterRepository : IShopLocaterRepository
    {
        private HttpClient _client;
        public ShopLocaterRepository()
        {
            _client = new HttpClient();
            var apiUri = WebConfigurationManager.AppSettings["AzureWebApiUri"];
            _client.BaseAddress = new Uri(apiUri);
            // Add an Accept header for JSON format.
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        //public IEnumerable<BodyShop> GetAll()
        //{
        //    var shops = new List<BodyShop>();
        //    HttpResponseMessage response = _client.GetAsync("api/Shop/").Result;
        //    try
        //    {
        //        //shops = response.Content.ReadAsAsync<IEnumerable<BodyShop>>().Result;
        //        JArray content = response.Content.ReadAsAsync<JArray>().Result;
        //        foreach (var _shop in content)
        //        {
        //            var shop = new BodyShop
        //               {
        //                   AddressLine1 = _shop.Value<string>("AddressLine1"),
        //                   AddressLine2 = _shop.Value<string>("AddressLine2"),
        //                   City = _shop.Value<string>("City"),
        //                   Contact = _shop.Value<string>("Contact"),
        //                   Country = _shop.Value<string>("Country"),
        //                   Id = _shop.Value<int>("Id"),
        //                   Name = _shop.Value<string>("Name"),
        //                   Pin = _shop.Value<string>("Pin"),
        //                   ProviderId = _shop.Value<int>("ProviderId"),
        //                   State = _shop.Value<string>("State")
        //               };
        //            JavaScriptSerializer jss = new JavaScriptSerializer();
        //            shop.ShopGeoLocation = jss.Deserialize<GeoLocation>(_shop.Value<object>("ShopGeoLocation").ToString());
        //            shops.Add(shop);
        //        }
        //    }
        //    catch (HttpResponseException httpEx)
        //    {
        //        switch (httpEx.Response.StatusCode)
        //        {
        //            case HttpStatusCode.NoContent:
        //                var errorResponse = new HttpResponseMessage(HttpStatusCode.NoContent);
        //                errorResponse.ReasonPhrase = httpEx.Message;
        //                throw new HttpResponseException(errorResponse);
        //            case System.Net.HttpStatusCode.NotFound:
        //                // not found 404 should return null object
        //                shops = null;
        //                break;
        //        }
        //    }

        //    return shops;
        //}

        //public BodyShop Get(int id)
        //{
        //    var shop = new BodyShop();
        //    HttpResponseMessage response = _client.GetAsync(string.Format("api/Shop/{0}", id)).Result;  // Blocking call!
        //    try
        //    {
        //        shop = response.Content.ReadAsAsync<BodyShop>().Result;
        //    }
        //    catch (HttpResponseException httpEx)
        //    {
        //        switch (httpEx.Response.StatusCode)
        //        {
        //            case System.Net.HttpStatusCode.NotFound:
        //                // not found 404 should return null object
        //                shop = null;
        //                break;
        //        }
        //    }
        //    return shop;
        //}

        //public BodyShop Add(BodyShop item)
        //{

        //    //var shop = new BodyShop();
        //    //HttpResponseMessage response = _client.PostAsync<BodyShop>(string.Format("api/Shop/"), item).Result;  // Blocking call!
        //    //try
        //    //{
        //    //    shop = response.Content.ReadAsAsync<BodyShop>().Result;
        //    //}
        //    //catch (HttpResponseException httpEx)
        //    //{
        //    //    switch (httpEx.Response.StatusCode)
        //    //    {
        //    //        case System.Net.HttpStatusCode.NotFound:
        //    //            // not found 404 should return null object
        //    //            shop = null;
        //    //            break;
        //    //    }
        //    //}
        //    return item;
        //}

        //public void Remove(int id)
        //{

        //}

        //public bool Update(BodyShop item)
        //{
        //    return true;
        //}

        public IList<BodyShop> GetProviderBodyShopsInRange(string providerName, GeoLocation currentLoc, int range)
        {
            var shops = new List<BodyShop>();
            HttpResponseMessage response = _client.GetAsync(string.Format("api/ShopsLocater/{0}/{1}/{2}/{3}", providerName, currentLoc.Latitude, currentLoc.Longitude, range)).Result;
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(response.Content.ToString()),
                    ReasonPhrase = response.ReasonPhrase
                });
            }
            else
            {
                JArray content = response.Content.ReadAsAsync<JArray>().Result;
                foreach (var item in content)
                {
                    var shop = new BodyShop
                        {
                            AddressLine1 = item.Value<string>("AddressLine1"),
                            AddressLine2 = item.Value<string>("AddressLine2"),
                            City = item.Value<string>("City"),
                            Contact = item.Value<string>("Contact"),
                            Country = item.Value<string>("Country"),
                            Id = item.Value<int>("Id"),
                            Name = item.Value<string>("Name"),
                            Pin = item.Value<string>("Pin"),
                            ProviderId = item.Value<int>("ProviderId"),
                            State = item.Value<string>("State")
                        };
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    shop.ShopGeoLocation = jss.Deserialize<GeoLocation>(item.Value<object>("ShopGeoLocation").ToString());

                    shops.Add(shop);
                }
            }
            return shops;
        }
    }
}