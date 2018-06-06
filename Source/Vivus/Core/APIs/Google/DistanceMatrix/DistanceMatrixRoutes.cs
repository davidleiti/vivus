namespace Vivus.Core.APIs.Google.DistanceMatrix
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of routes.
    /// </summary>
    public class DistanceMatrixRoutes
    {
        /// <summary>
        /// Gets the distance and duration for every route.
        /// </summary>
        [JsonProperty("elements")]
        public IList<DistanceMatrixRoute> Routes { get; private set; }
    }
}
