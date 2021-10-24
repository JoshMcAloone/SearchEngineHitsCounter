using System.Net;

namespace SearchEngineHitCounter.Extensions
{
    public static class HttpStatusCodeExtensions
    {
        public static bool IsSuccessStatusCode(this HttpStatusCode source)
        {
            return (int)source >= 200 && (int)source <= 299;
        }
    }
}
