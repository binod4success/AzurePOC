using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BodyShopsApp.ViewModels.Shop
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            _shopList = new List<BodyShopModel>(0);
        }

        private IList<BodyShopModel> _shopList;

        public IList<BodyShopModel> Shops { get { return _shopList; } set { _shopList = value; } }
    }
}