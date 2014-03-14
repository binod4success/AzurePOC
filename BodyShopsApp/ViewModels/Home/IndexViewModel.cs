using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BodyShopsApp.ViewModels.Home
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            _shopList = new List<BodyShopModel>(0);
        }

        private IList<BodyShopModel> _shopList;

        [Required(ErrorMessage = "Please enter the Provider name.")]
        [Display(Name = "Provider Name")]
        public string ProviderName { get; set; }

        [Display(Name = "Range")]
        public int Range { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public IEnumerable<SelectListItem> RangeList
        {
            get
            {
                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem() { Text = "05", Value = "05" });
                list.Add(new SelectListItem() { Text = "10", Value = "10" });
                list.Add(new SelectListItem() { Text = "15", Value = "15" });
                list.Add(new SelectListItem() { Text = "20", Value = "20" });
                list.Add(new SelectListItem() { Text = "30", Value = "30" });
                list.Add(new SelectListItem() { Text = "40", Value = "40" });
                list.Add(new SelectListItem() { Text = "50", Value = "50" });
                list.Add(new SelectListItem() { Text = "60", Value = "60" });
                list.Add(new SelectListItem() { Text = "70", Value = "70" });
                list.Add(new SelectListItem() { Text = "80", Value = "80" });
                list.Add(new SelectListItem() { Text = "90", Value = "90" });
                list.Add(new SelectListItem() { Text = "100", Value = "100" });
                list.Add(new SelectListItem() { Text = "All", Value = "100000" });
                return list.ToArray();
            }
        }

        public IList<BodyShopModel> Shops
        {
            get
            {
                return _shopList;
            }
            set
            {
                _shopList = value ?? new List<BodyShopModel>(0);
            }
        }
    }
}