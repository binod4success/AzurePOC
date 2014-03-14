using BodyShopsWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BodyShopsWebApi.Repository
{
    public interface IShopRepository
    {
        /// <summary>
        /// Returns the list of All Body shops
        /// </summary>
        /// <returns></returns>
        IEnumerable<Shop> GetAll();

        /// <summary>
        /// Returns a Body Shop which ID is passed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Shop Get(int id);

        /// <summary>
        /// Add a new Body Shop
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Add(Shop item);

        /// <summary>
        /// Delete the Body Shop which ID was passed
        /// </summary>
        /// <param name="id"></param>
        bool Remove(int id);

        /// <summary>
        /// Update the info of Body Shop
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Update(Shop item);

        /// <summary>
        /// Returns the providerId for the passed provider name by user
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        int? GetCliamProvider(string providerName);
                
        /// <summary>
        /// Returns the list of body shops for a given Claim ID which are near by passed current location in the range passed.
        /// </summary>
        /// <param name="providerId">providerId for the passed Provider Name by User</param>
        /// <param name="currentLoc">User Current Location</param>
        /// <param name="range">Distance from current location and body shop location</param>
        /// <returns>Returns the list of Body shops in the paased range of current location of User</returns>
        IList<Shop> GetProviderBodyShopsInRange(int providerId, GeoLocation currentLoc, int range);
    }
}
