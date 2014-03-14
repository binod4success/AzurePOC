using System;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using BodyShopsApp.Models;
using BodyShopsApp.Repository;
using BodyShopsApp.ViewModels;
using BodyShopsApp.ViewModels.Home;

namespace BodyShopsApp.Controllers
{
    public class HomeController : Controller
    {
        private static readonly IShopLocaterRepository _repos = new ShopLocaterRepository();

        public ActionResult Index()
        {
            var model = new IndexViewModel();
            return View("Index", model);
        }

        public JsonResult GetBodyShop(IndexViewModel model)
        {
            try
            {
                var shops = _repos.GetProviderBodyShopsInRange(model.ProviderName, new GeoLocation(model.Latitude, model.Longitude), model.Range);                
                foreach (var item in shops)
                {
                    model.Shops.Add(new BodyShopModel
                    {
                        Id = item.Id,
                        AddressLine1 = item.AddressLine1,
                        AddressLine2 = item.AddressLine2,
                        City = item.City,
                        Contact = item.Contact,
                        Country = item.Country,
                        Name = item.Name,
                        Pin = item.Pin,
                        ProviderId = item.ProviderId,
                        State = item.State,
                        ShopGeoLocation = new ViewModels.GeoLocationModel
                        {
                            Latitude = item.ShopGeoLocation.Latitude,
                            Longitude = item.ShopGeoLocation.Longitude
                        }
                    });
                }
            }
            catch (HttpResponseException ex)
            {
                switch (ex.Response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        Response.StatusCode = (int)HttpStatusCode.NotFound;
                        Response.AddHeader("errorMsg", String.Format("DRP Shops of {0} doesn't exist in specified {1} K.M range.", model.ProviderName, model.Range));
                        break;
                    default:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        Response.AddHeader("errorMsg", ex.Response.ReasonPhrase);
                        break;
                }
            }
            return Json(model.Shops, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}
