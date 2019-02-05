using System.Net;

namespace ToText.API
{
    public class Constants
    {
        public const string BASE_URL = "http://www.to-text.net";
        public const string DOWNLOAD_URL = "http://www.to-text.net/?action=download&f={0}";

        public static readonly string[] SUPPORTED_FORMATS = { "tif", "jpg", "bmp", "png", "pdf" };
        public const long MAX_SIZE = 5242880;

        public static readonly WebHeaderCollection DefaultHeaders = new WebHeaderCollection()
        {
            { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8" },
            { "Accept-Encoding", "gzip, deflate" },
            { "Accept-Language", "en-US,en" },
            { "User-Agent", "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36" },
            { "Origin", "http://www.to-text.net" },
            { "Host", "www.to-text.net" }
        };

    }
}
