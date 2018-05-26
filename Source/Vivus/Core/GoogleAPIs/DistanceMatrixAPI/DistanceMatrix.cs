namespace Vivus.Core.GoogleAPIs.DistanceMatrixAPI
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a JSON object returned by the Google Distance Matrix API.
    /// </summary>
    public class DistanceMatrix
    {
        /// <summary>
        /// Gets the destination addresses.
        /// </summary>
        [JsonProperty("destination_addresses")]
        public IList<string> DestinationAddresses { get; private set; }

        /// <summary>
        /// Gets the origin addresses.
        /// </summary>
        [JsonProperty("origin_addresses")]
        public IList<string> OriginAddresses { get; private set; }

        /// <summary>
        /// Gets the distance and duration between each of the origin and destination addresses.
        /// </summary>
        [JsonProperty("rows")]
        public IList<DistanceMatrixRoutes> Results { get; private set; }
    }
}
