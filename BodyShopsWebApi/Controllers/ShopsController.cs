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
    public class ShopsController : ApiController
    {
        private static readonly IShopRepository repository = new ShopRepository();

        // GET api/shops
        public IEnumerable<Shop> Get()
        {
            return repository.GetAll();
        }

        // GET api/shops/5
        public Shop Get(int id)
        {
            Shop item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        // POST api/shops
        public bool Post(Shop item)
        {
            return repository.Add(item);
        }

        // PUT api/shops/5
        public bool Put(int id, Shop shop)
        {
            shop.Id = id;
            return repository.Update(shop);
        }

        // DELETE api/shops/5
        public bool Delete(int id)
        {
           return repository.Remove(id);
        }
    }
}