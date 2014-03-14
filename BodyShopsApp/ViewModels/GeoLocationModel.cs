
namespace BodyShopsApp.ViewModels
{
    public class GeoLocationModel
    {
        private double _longitude;
        private double _latitude;

        public GeoLocationModel() { }

        public GeoLocationModel(double lat, double lng)
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