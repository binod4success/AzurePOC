using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BodyShopsApp.Models
{
    public class GeoLocation
    {
        private double _longitude;
        private double _latitude;

        public GeoLocation() { }

        public GeoLocation(double lat, double lng)
        {
            _longitude = lng;
            _latitude = lat;
        }

        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
            }
        }

        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
            }
        }
    }
}