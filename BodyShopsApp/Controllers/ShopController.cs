using System.Net;

using System.Web.Mvc;
using BodyShopsApp.Filters;
using BodyShopsApp.Models;
using BodyShopsApp.Repository;
using BodyShopsApp.ViewModels;
using BodyShopsApp.ViewModels.Shop;


namespace BodyShopsApp.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class ShopController : Controller
    {
        private static readonly IShopRepository _repos = new ShopRepository();
                
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            try
            {
                var shops = _repos.GetAll();
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
                        ProviderName = item.ProviderName,
                        State = item.State,
                        ShopGeoLocation = new ViewModels.GeoLocationModel
                        {
                            Latitude = item.ShopGeoLocation.Latitude,
                            Longitude = item.ShopGeoLocation.Longitude
                        }
                    });
                }
            }
            catch (System.Web.Http.HttpResponseException ex)
            {
                switch (ex.Response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        ModelState.AddModelError("", "DRP Shops not found.");
                        break;
                    default:
                        ModelState.AddModelError("errorMsg", ex.Response.ReasonPhrase);
                        break;
                }
            }
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ViewBag.Title = "Edit Shop Details";
            var model = new ShopEditViewModel();
            if (id == null)
            {
                ModelState.AddModelError("id", "Can't show record to edit, ID is required.");
                return RedirectToAction("Index");
            }
            try
            {
                var shop = _repos.Get(id.Value);
                model.Shop = new BodyShopModel
                {
                    Id = shop.Id,
                    AddressLine1 = shop.AddressLine1,
                    AddressLine2 = shop.AddressLine2,
                    City = shop.City,
                    Contact = shop.Contact,
                    Country = shop.Country,
                    Name = shop.Name,
                    Pin = shop.Pin,
                    ProviderId = shop.ProviderId,
                    State = shop.State,
                    ShopGeoLocation = new ViewModels.GeoLocationModel
                    {
                        Latitude = shop.ShopGeoLocation.Latitude,
                        Longitude = shop.ShopGeoLocation.Longitude
                    }
                };
            }
            catch (System.Web.Http.HttpResponseException ex)
            {
                switch (ex.Response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        ModelState.AddModelError("", "DRP Shops not found.");
                        break;
                    default:
                        ModelState.AddModelError("errorMsg", ex.Response.ReasonPhrase);
                        break;
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Title = "Add New Shop";
            return View(new ShopEditViewModel());
        }

        [HttpPost]
        public ActionResult SaveShopData(ShopEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please provide all required information before update.");
            }
            var shop = new BodyShop
            {
                Id = model.Shop.Id.Value,
                AddressLine1 = model.Shop.AddressLine1,
                AddressLine2 = model.Shop.AddressLine2,
                City = model.Shop.City,
                Contact = model.Shop.Contact,
                Country = model.Shop.Country,
                Name = model.Shop.Name,
                Pin = model.Shop.Pin,
                ProviderId = model.Shop.ProviderId,
                ShopGeoLocation = new GeoLocation
                {
                    Latitude = model.Shop.ShopGeoLocation.Latitude,
                    Longitude = model.Shop.ShopGeoLocation.Longitude
                },
                State = model.Shop.State
            };

            var result = _repos.Update(shop.Id, shop);
            TempData["StatusMessage"] = result ? "Details updated successfully." : "Error: Can't update details.";
            return RedirectToAction("Edit", new { id = model.Shop.Id });
        }

        [HttpPost]
        public ActionResult AddNewShop(ShopEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please provide all required information before add/update.");
            }
            var shop = new BodyShop
            {
                AddressLine1 = model.Shop.AddressLine1,
                AddressLine2 = model.Shop.AddressLine2,
                City = model.Shop.City,
                Contact = model.Shop.Contact,
                Country = model.Shop.Country,
                Name = model.Shop.Name,
                Pin = model.Shop.Pin,
                ProviderId = model.Shop.ProviderId,
                ShopGeoLocation = new GeoLocation
                {
                    Latitude = model.Shop.ShopGeoLocation.Latitude,
                    Longitude = model.Shop.ShopGeoLocation.Longitude
                },
                State = model.Shop.State
            };
            var result = _repos.Add(shop);
            TempData["StatusMessage"] = result ? "Details added successfully." : "Error: Can't add details.";
            return RedirectToAction("Add");
        }
        
        [HttpPost]
        public ActionResult Remove(int? id)
        {
            if (id == null)
            {
                TempData["StatusMessage"] = "Shop ID is required.";
            }
            else
            {
                var result = _repos.Remove(id.Value);
                TempData["StatusMessage"] = result ? "Shop detail removed successfully." : "Error: Can't delete.";
            }
            return RedirectToAction("Index");
        }
    }
}
