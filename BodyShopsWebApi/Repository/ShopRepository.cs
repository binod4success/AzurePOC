using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BodyShopsWebApi.Models;

namespace BodyShopsWebApi.Repository
{
    public class ShopRepository : IShopRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

        #region Calculate Distance

        const double PIx = 3.141592653589793;
        const double RADIUS = 6378.16;

        public static double Radians(double x)
        {
            return x * PIx / 180;
        }

        public static double DistanceBetweenPlaces(double lat1, double lon1, double lat2, double lon2)
        {
            double dlon = Radians(lon2 - lon1);
            double dlat = Radians(lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = angle * RADIUS;
            return distance;
        }

        #endregion

        public static void AddParameter(ref SqlCommand cmd, string parameterName, SqlDbType dataType, object value)
        {
            SqlParameter param = cmd.Parameters.Add(parameterName, dataType);
            if (value == null || (value is string && string.IsNullOrWhiteSpace(value.ToString())))
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = value;
            }
        }

        /// <summary>
        /// Returns the list of All Body shops
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Shop> GetAll()
        {
            const string QUERY = @"
                         SELECT BS.ShopId,
                                BS.ProviderId,
                                BS.Title,
                                BS.AddressLine1,
                                BS.AddressLine2,
                                BS.City,
                                BS.State,
                                BS.Country,
                                BS.Pin,
                                BS.Contact,
                                BS.Latitude,
                                BS.Longitude,
                                PRD.Name 
                           FROM BodyShop BS
                          INNER JOIN Provider PRD
                             ON PRD.ProviderId = BS.ProviderId
                          ORDER BY PRD.Name,BS.Title
                ";

            var shops = new List<Shop>();
            using (SqlConnection _dbConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(QUERY, _dbConnection);
                _dbConnection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var data = new Shop
                    {
                        Id = Int32.Parse(reader["ShopId"].ToString()),
                        ProviderId = Int32.Parse(reader["ProviderId"].ToString()),
                        ProviderName = reader["Name"].ToString(),
                        Name = reader["Title"].ToString(),
                        AddressLine1 = reader["AddressLine1"].ToString(),
                        AddressLine2 = reader["AddressLine2"].ToString(),
                        City = reader["City"].ToString(),
                        State = reader["State"].ToString(),
                        Country = reader["Country"].ToString(),
                        Pin = reader["Pin"].ToString(),
                        Contact = reader["Contact"].ToString(),
                        ShopGeoLocation = new GeoLocation
                        {
                            Latitude = Double.Parse(reader["Latitude"].ToString()),
                            Longitude = Double.Parse(reader["Longitude"].ToString())
                        }
                    };
                    shops.Add(data);
                }
            }
            return shops;
        }

        /// <summary>
        /// Returns a Body Shop which ID is passed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Shop Get(int id)
        {
            string QUERY = @"
                         SELECT ShopId,
                                ProviderId,
                                Title,
                                AddressLine1,
                                AddressLine2,
                                City,
                                State,
                                Country,
                                Pin,
                                Contact,
                                Latitude,
                                Longitude 
                           FROM BodyShop
                          WHERE ShopId = @ShopId";
            var shops = new List<Shop>();
            using (SqlConnection _dbConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(QUERY, _dbConnection);
                cmd.Parameters.Add("@ShopId", SqlDbType.Int).Value = id;
                _dbConnection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var data = new Shop
                    {
                        Id = Int32.Parse(reader["ShopId"].ToString()),
                        ProviderId = Int32.Parse(reader["ProviderId"].ToString()),
                        Name = reader["Title"].ToString(),
                        AddressLine1 = reader["AddressLine1"].ToString(),
                        AddressLine2 = reader["AddressLine2"].ToString(),
                        City = reader["City"].ToString(),
                        State = reader["State"].ToString(),
                        Country = reader["Country"].ToString(),
                        Pin = reader["Pin"].ToString(),
                        Contact = reader["Contact"].ToString(),
                        ShopGeoLocation = new GeoLocation
                        {
                            Latitude = Double.Parse(reader["Latitude"].ToString()),
                            Longitude = Double.Parse(reader["Longitude"].ToString())
                        }
                    };
                    shops.Add(data);
                }
            }
            return shops.FirstOrDefault(); ;
        }

        /// <summary>
        /// Add a new Body Shop
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Add(Shop item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            string QUERY = @"
                INSERT INTO BodyShop
                       (ProviderId,
                        Title,
                        AddressLine1,
                        AddressLine2,
                        City,
                        State,
                        Country,
                        Pin,
                        Latitude,
                        Longitude,
                        Contact)
                 VALUES
                       (@ProviderId,
                       @Title,
                       @AddressLine1,
                       @AddressLine2,
                       @City,
                       @State,
                       @Country,
                       @Pin,
                       @Latitude,
                       @Longitude,
                       @Contact)
        ";
            using (SqlConnection _dbConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(QUERY, _dbConnection);
                                
                AddParameter(ref cmd, "@ProviderId", SqlDbType.Int, item.ProviderId);
                AddParameter(ref cmd, "@Title", SqlDbType.VarChar, item.Name);
                AddParameter(ref cmd, "@AddressLine1", SqlDbType.VarChar, item.AddressLine1);
                AddParameter(ref cmd, "@AddressLine2", SqlDbType.VarChar, item.AddressLine2);
                AddParameter(ref cmd, "@City", SqlDbType.VarChar, item.City);
                AddParameter(ref cmd, "@State", SqlDbType.VarChar, item.State);
                AddParameter(ref cmd, "@Country", SqlDbType.VarChar, item.Country);
                AddParameter(ref cmd, "@Pin", SqlDbType.VarChar, item.Pin);
                AddParameter(ref cmd, "@Contact", SqlDbType.VarChar, item.Contact);
                AddParameter(ref cmd, "@Latitude", SqlDbType.Decimal, item.ShopGeoLocation.Latitude);
                AddParameter(ref cmd, "@Longitude", SqlDbType.Decimal, item.ShopGeoLocation.Longitude);

                _dbConnection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Delete the Body Shop which ID was passed
        /// </summary>
        /// <param name="id"></param>
        public bool Remove(int id)
        {
            const string QUERY = @" DELETE FROM BodyShop WHERE ShopId = @ShopId";
            using (SqlConnection _dbConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(QUERY, _dbConnection);
                cmd.Parameters.Add("@ShopId", SqlDbType.Int).Value = id;

                _dbConnection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Update the info of Body Shop
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Update(Shop item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            string QUERY = @"
                UPDATE BodyShop
                   SET [ProviderId] = @ProviderId,
                       [Title] = @Title,
                       [AddressLine1] = @AddressLine1,
                       [AddressLine2] = @AddressLine2,
                       [City] = @City,
                       [State] = @State,
                       [Country] = @Country,
                       [Pin] = @Pin,
                       [Latitude] = @Latitude,
                       [Longitude] = @Longitude,
                       [Contact] = @Contact
                 WHERE [ShopId] = @ShopId
            ";
            using (SqlConnection _dbConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(QUERY, _dbConnection);

                AddParameter(ref cmd, "@ShopId", SqlDbType.Int, item.Id);
                AddParameter(ref cmd, "@ProviderId", SqlDbType.Int, item.ProviderId);
                AddParameter(ref cmd, "@Title", SqlDbType.VarChar, item.Name);
                AddParameter(ref cmd, "@AddressLine1", SqlDbType.VarChar, item.AddressLine1);
                AddParameter(ref cmd, "@AddressLine2", SqlDbType.VarChar, item.AddressLine2);
                AddParameter(ref cmd, "@City", SqlDbType.VarChar, item.City);
                AddParameter(ref cmd, "@State", SqlDbType.VarChar, item.State);
                AddParameter(ref cmd, "@Country", SqlDbType.VarChar, item.Country);
                AddParameter(ref cmd, "@Pin", SqlDbType.VarChar, item.Pin);
                AddParameter(ref cmd, "@Contact", SqlDbType.VarChar, item.Contact);
                AddParameter(ref cmd, "@Latitude", SqlDbType.Decimal, item.ShopGeoLocation.Latitude);
                AddParameter(ref cmd, "@Longitude", SqlDbType.Decimal, item.ShopGeoLocation.Longitude);

                _dbConnection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }


        /// <summary>
        /// Returns the Claim provider ID
        /// </summary>
        /// <param name="claimId">Claim ID passed by User</param>
        /// <returns>Claim Provider ID</returns>
        public int? GetCliamProvider(string providerName)
        {
            string QUERY = string.Format(@"
                         SELECT TOP (1) ProviderId
                           FROM Provider
                          WHERE Name LIKE '%{0}%' ", providerName);

            using (SqlConnection _dbConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(QUERY, _dbConnection);
                _dbConnection.Open();
                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    return Int32.Parse(result.ToString());
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns the list of Body shops of passed provider
        /// </summary>
        /// <param name="CliamProviderId">Claim provider Id for which user claim is registered</param>        
        /// <returns>List of Body shops for which User claim is valid to process</returns>
        private IList<Shop> GetBodyShops(int cliamProviderId)
        {
            const string QUERY = @"
                         SELECT ShopId,
                                ProviderId,
                                Title,
                                AddressLine1,
                                AddressLine2,
                                City,
                                State,
                                Country,
                                Pin,
                                Contact,
                                Latitude,
                                Longitude 
                           FROM BodyShop
                          WHERE ProviderId = @ProviderId
                ";

            var shops = new List<Shop>();
            using (SqlConnection _dbConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(QUERY, _dbConnection);
                cmd.Parameters.Add("@ProviderId", SqlDbType.Int).Value = cliamProviderId;
                _dbConnection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var data = new Shop
                    {
                        Id = Int32.Parse(reader["ShopId"].ToString()),
                        ProviderId = Int32.Parse(reader["ProviderId"].ToString()),
                        Name = reader["Title"].ToString(),
                        AddressLine1 = reader["AddressLine1"].ToString(),
                        AddressLine2 = reader["AddressLine2"].ToString(),
                        City = reader["City"].ToString(),
                        State = reader["State"].ToString(),
                        Country = reader["Country"].ToString(),
                        Pin = reader["Pin"].ToString(),
                        Contact = reader["Contact"].ToString(),
                        ShopGeoLocation = new GeoLocation
                        {
                            Latitude = Double.Parse(reader["Latitude"].ToString()),
                            Longitude = Double.Parse(reader["Longitude"].ToString())
                        }
                    };
                    shops.Add(data);
                }
            }
            return shops;
        }

        /// <summary>
        /// Filters out the body shops from a given list near by current location in the passed range
        /// </summary>
        /// <param name="ShopList">List of Body shops</param>
        /// <param name="currentLoc">User Current Location</param>
        /// <param name="range">Distance from current location and body shop location</param>
        /// <returns>Returns the list of Body shops in the paased range with current location of User</returns>
        private IList<Shop> GetBodyShopsInRange(IList<Shop> ShopList, GeoLocation currentLoc, int range)
        {
            var shopsInRange = ShopList.Where(p => DistanceBetweenPlaces(currentLoc.Latitude, currentLoc.Longitude, p.ShopGeoLocation.Latitude, p.ShopGeoLocation.Longitude) <= range);
            return shopsInRange.ToList();
        }

        /// <summary>
        /// Returns the list of body shops for a given Claim ID which are near by passed current location in the range passed.
        /// </summary>
        /// <param name="providerName">Provider Name passed by User</param>
        /// <param name="currentLoc">User Current Location</param>
        /// <param name="range">Distance from current location and body shop location</param>
        /// <returns>Returns the list of Body shops in the paased range of current location of User</returns>
        public IList<Shop> GetProviderBodyShopsInRange(int cliamProviderId, GeoLocation currentLoc, int range)
        {
            IList<Shop> shopList = GetBodyShops(cliamProviderId);
            return GetBodyShopsInRange(shopList, currentLoc, range);
        }
    }
}