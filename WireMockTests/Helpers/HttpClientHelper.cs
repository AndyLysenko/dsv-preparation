using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WireMockTests.Helpers
{
    public static class HttpClientHelper
    {
        private static readonly HttpClient client = new HttpClient();

        static HttpClientHelper()
        {
        }

        public static async Task<string> GetAsync(string uri)
        {
            return await client.GetStringAsync(uri);
        }

        public static string Get(string uri, Dictionary<string, string> headers = null )
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            headers?.ToList().ForEach(h => request.Headers.Add(h.Key, h.Value));

            //request.Headers.Add("TestHeader", "TestHeaderValue");

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();

            }
        }
    }
}
