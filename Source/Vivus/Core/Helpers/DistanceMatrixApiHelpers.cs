namespace Vivus.Core.Helpers
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Vivus.Core.GoogleAPIs.DistanceMatrixAPI;
    using Vivus.Core.Model;

    /// <summary>
    /// Represents a collection of methods to simplify the work with the Google DistanceMatrix API.
    /// </summary>
    public static class DistanceMatrixApiHelpers
    {
        #region Private Constants

        /// <summary>
        /// The url to the API.
        /// </summary>
        private const string API_URL = "http://maps.googleapis.com/maps/api/distancematrix/json";

        /// <summary>
        /// The mode of transport to use when calculating distance.
        /// Indicates distance calculation using the road network.
        /// </summary>
        private const string API_MODE = "driving";

        /// <summary>
        /// The language in which to return results.
        /// </summary>
        private const string API_LANGUAGE = "en-US";

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a collection of all the distances and durations with their origin and destination addresses.
        /// </summary>
        /// <param name="originAddress">The origin address.</param>
        /// <param name="destinationAddresses">A collection of destination addresses.</param>
        /// <returns></returns>
        public static IEnumerable<RouteDetails> GetDistances(this Address originAddress, IEnumerable<Address> destinationAddresses)
        {
            string apiResponse, orgAddress, destAddresses;
            Task<string> getAsync;
            IList<DistanceMatrixRoute> routes;
            List<Address> destAddressesList;
            
            // Prepare the origin address to be sent as parameter to the get request
            orgAddress = $"{ originAddress.Street.Trim().Replace(' ', '+') }+{ originAddress.StreetNo.Trim().Replace(' ', '+') },+{ originAddress.City.Trim().Replace(' ', '+') }+Romania";
            // Prepare the destination addresses to be sent as parameter to the get request
            destAddresses = destinationAddresses.ToList().Select(addr => $"{ addr.Street.Trim().Replace(' ', '+') }+{ addr.StreetNo.Trim().Replace(' ', '+') },+{ addr.City.Trim().Replace(' ', '+') }+Romania").Aggregate((addr1, addr2) => $"{ addr1 }|{ addr2 }");
            // Send the request and wait for the result
            getAsync = Http.Requests.GetAsync($"{ API_URL }?origins={ orgAddress }&destinations={ destAddresses }&mode={ API_MODE }&language={ API_LANGUAGE }");
            getAsync.Wait();
            apiResponse = getAsync.Result;
            // Convert the request to a more understandable format and get the routes
            routes = JsonConvert.DeserializeObject<DistanceMatrix>(apiResponse).Results[0].Routes;

            // Convert the destionation addresses enumerable to a list.
            destAddressesList = destinationAddresses.ToList();

            // Add to the results the routes with their distance and duration
            for (int i = 0; i < destAddressesList.Count; i++)
                if (routes[i].Status == "OK")
                    yield return new RouteDetails
                    {
                        OriginAddress = originAddress,
                        DestinationAddress = destAddressesList[i],
                        Distance = routes[i].Distance.Value,
                        Duration = routes[i].Duration.Value
                    };
        }

        #endregion

        #region Inner Classes

        /// <summary>
        /// Represents an entity returned by the <see cref="GetDistances(Address, IEnumerable{Address})"/> method.
        /// </summary>
        public class RouteDetails
        {
            /// <summary>
            /// Gets or sets the origin address.
            /// </summary>
            public Address OriginAddress { get; set; }

            /// <summary>
            /// Gets or sets the destination address.
            /// </summary>
            public Address DestinationAddress { get; set; }

            /// <summary>
            /// Gets or sets the distance of the route.
            /// </summary>
            public int Distance { get; set; }

            /// <summary>
            /// Gets or sets the duration of the route.
            /// </summary>
            public int Duration { get; set; }
        }

        #endregion
    }
}
