using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;
using BodyShopsApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace BodyShopsApp.Repository
{
    public class ShopRepository : IShopRepository
    {
        private HttpClient _client;
        public ShopRepository()
        {
            _client = new HttpClient();
            var apiUri = WebConfigurationManager.AppSettings["AzureWebApiUri"];
            _client.BaseAddress = new Uri(apiUri);
            // Add an Accept header for JSON format.
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public IEnumerable<BodyShop> GetAll()
        {
            var shops = new List<BodyShop>();
            HttpResponseMessage response = _client.GetAsync("api/shops/").Result;
            try
            {
                //shops = response.Content.ReadAsAsync<IEnumerable<BodyShop>>().Result;
                JArray content = response.Content.ReadAsAsync<JArray>().Result;
                foreach (var _shop in content)
                {
                    var shop = new BodyShop
                       {
                           AddressLine1 = _shop.Value<string>("AddressLine1"),
                           AddressLine2 = _shop.Value<string>("AddressLine2"),
                           City = _shop.Value<string>("City"),
                           Contact = _shop.Value<string>("Contact"),
                           Country = _shop.Value<string>("Country"),
                           Id = _shop.Value<int>("Id"),
                           Name = _shop.Value<string>("Name"),
                           Pin = _shop.Value<string>("Pin"),
                           ProviderId = _shop.Value<int>("ProviderId"),
                           ProviderName = _shop.Value<string>("ProviderName"),
                           State = _shop.Value<string>("State")
                       };
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    shop.ShopGeoLocation = jss.Deserialize<GeoLocation>(_shop.Value<object>("ShopGeoLocation").ToString());
                    shops.Add(shop);
                }
            }
            catch (HttpResponseException httpEx)
            {
                switch (httpEx.Response.StatusCode)
                {
                    case HttpStatusCode.NoContent:
                        var errorResponse = new HttpResponseMessage(HttpStatusCode.NoContent);
                        errorResponse.ReasonPhrase = httpEx.Message;
                        throw new HttpResponseException(errorResponse);
                    case System.Net.HttpStatusCode.NotFound:
                        // not found 404 should return null object
                        shops = null;
                        break;
                }
            }

            return shops;
        }

        public BodyShop Get(int id)
        {
            var shop = new BodyShop();
            HttpResponseMessage response = _client.GetAsync(string.Format("api/shops/{0}", id)).Result;  // Blocking call!
            try
            {
                shop = response.Content.ReadAsAsync<BodyShop>().Result;
            }
            catch (HttpResponseException httpEx)
            {
                switch (httpEx.Response.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        // not found 404 should return null object
                        shop = null;
                        break;
                }
            }
            return shop;
        }

        public bool Add(BodyShop item)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringContent content = new StringContent(jss.Serialize(item), Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = _client.PostAsync("api/shops/", content).Result;
                return Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Remove(int shopId)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync(string.Format("api/shops/{0}", shopId)).Result;
                return Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(int shopId, BodyShop item)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringContent content = new StringContent(jss.Serialize(item), Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = _client.PutAsync(string.Format("api/shops/{0}", shopId), content).Result;
                return Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}