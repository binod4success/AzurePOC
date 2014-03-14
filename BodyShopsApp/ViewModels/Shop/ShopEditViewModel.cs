using System.Collections.Generic;
using System.Web.Mvc;

namespace BodyShopsApp.ViewModels.Shop
{
    public class ShopEditViewModel
    {
        public ShopEditViewModel()
        {
            IList<SelectListItem> _list = new List<SelectListItem>(0);
            _list.Add(new SelectListItem() { Text = "Progressive", Value = "1" });
            _list.Add(new SelectListItem() { Text = "State Farm", Value = "2" });
            _list.Add(new SelectListItem() { Text = "MPI", Value = "3" });
            _providerList = _list;
        }

        public BodyShopModel Shop { get; set; }

        private IEnumerable<SelectListItem> _providerList;

        public IEnumerable<SelectListItem> ProviderList
        {
            get
            {
                return _providerList;
            }
            set
            {
                _providerList = value;
            }
        }
    }
}