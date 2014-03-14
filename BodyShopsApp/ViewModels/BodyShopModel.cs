using System.ComponentModel.DataAnnotations;

namespace BodyShopsApp.ViewModels
{
    public class BodyShopModel
    {
        public BodyShopModel()
        {
            ShopGeoLocation = new GeoLocationModel();
        }
        public int? Id { get; set; }

        [Display(Name = "Provider")]
        [Required]
        public int ProviderId { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Address Line1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line2")]
        public string AddressLine2 { get; set; }
        
        public string State { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Pin { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Contact { get; set; }

        public GeoLocationModel ShopGeoLocation { get; set; }

        [Display(Name = "Provider Name")]
        public string ProviderName { get; set; }
    }
}