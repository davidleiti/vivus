namespace Vivus.Core.APIs.Google.DistanceMatrix
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a class that stores the distance and duration details about a route.
    /// </summary>
    public class DistanceMatrixRoute
    {
        /// <summary>
        /// Gets the total distance of the current route, expressed in meters.
        /// </summary>
        [JsonProperty("distance")]
        public DistanceMatrixRouteEntity Distance { get; private set; }

        /// <summary>
        /// Gets the length of time it takes to travel the current route, expressed in seconds.
        /// </summary>
        [JsonProperty("duration")]
        public DistanceMatrixRouteEntity Duration { get; private set; }

        /// <summary>
        /// Gets the metadata of the request.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; private set; }
    }

    /// <summary>
    /// Represents a basic element for the <see cref="DistanceMatrixRoute"/> class.
    /// </summary>
    public class DistanceMatrixRouteEntity
    {
        /// <summary>
        /// Gets the text value.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; private set; }

        /// <summary>
        /// Gets the integer value.
        /// </summary>
        [JsonProperty("value")]
        public int Value { get; private set; }
    }
}
