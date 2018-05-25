namespace Vivus.Core.Http
{
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a collection of methods to handle http requests.
    /// </summary>
    public static class Requests
    {
        /// <summary>
        /// Represents an http GET request to an url.
        /// </summary>
        /// <param name="url">The url to send the request to.</param>
        /// <returns></returns>
        public static async Task<string> GetAsync(string url)
        {
            HttpWebRequest request;

            request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
                return await reader.ReadToEndAsync();
        }
    }
}
