using HtmlAgilityPack;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ToText.API.Exceptions;
using ToText.API.Utilities;

namespace ToText.API
{
    public class ToTextClient
    {

        private static Dictionary<Languages, string> LanguagesKeys;
        private static bool LanguagesLoaded { get { return LanguagesKeys?.Count > 0; } }

        private CookieContainer cookies;

        private readonly FileUpload fileUpload;

        public ToTextClient()
        {
            cookies = new CookieContainer();

            fileUpload = new FileUpload(this);
        }


        private void LoadLanguageKeys()
        {
            if (!LanguagesLoaded)
                LanguagesKeys = new LanguagesLoader(this).Load();
        }

        private async Task LoadLanguageKeysAsync()
        {
            if (!LanguagesLoaded)
                LanguagesKeys = await new LanguagesLoader(this).LoadAsync();
        }


        public string Convert(Image image, Languages language)
        {
            return ConvertFile(new File(image), language);
        }

        public string Convert(byte[] fileBytes, Languages language)
        {
            return ConvertFile(new File(fileBytes), language);
        }

        public string Convert(string file_path, Languages language)
        {
            return ConvertFile(new File(file_path), language);
        }

        public async Task<string> ConvertAsync(Image image, Languages language)
        {
            var file = await Task.Run(() => new File(image));
            return await ConvertFileAsync(file, language);
        }

        public async Task<string> ConvertAsync(byte[] fileBytes, Languages language)
        {
            var file = await Task.Run(() => new File(fileBytes));
            return await ConvertFileAsync(file, language);
        }

        public async Task<string> ConvertAsync(string file_path, Languages language)
        {
            var file = await Task.Run(() => new File(file_path));
            return await ConvertFileAsync(file, language);
        }


        private string ConvertFile(File file, Languages language)
        {
            if (file.Size > Constants.MAX_SIZE)
                throw new FileMaxSizeExceededException(file);

            if (!Constants.SUPPORTED_FORMATS.Contains(file.Format))
                throw new FileFormatNotSupportedException(file);

            LoadLanguageKeys();

            var response = fileUpload.Upload(file, LanguagesKeys[language]);

            if (!CheckIfConverted(response))
                throw new FileConversionErrorException(file);

            return DownloadFile(GenerateDownloadLink(response));
        }

        private async Task<string> ConvertFileAsync(File file, Languages language)
        {
            if (file.Size > Constants.MAX_SIZE)
                throw new FileMaxSizeExceededException(file);

            if (!Constants.SUPPORTED_FORMATS.Contains(file.Format))
                throw new FileFormatNotSupportedException(file);

            await LoadLanguageKeysAsync();

            var response = await fileUpload.UploadAsync(file, LanguagesKeys[language]);

            return await Task.Run(() =>
            {
                if (!CheckIfConverted(response))
                    throw new FileConversionErrorException(file);

                return DownloadFile(GenerateDownloadLink(response));
            });
        }



        private bool CheckIfConverted(string upload_resopnse)
        {
            return !upload_resopnse.Contains("Failed to convert");
        }

        private string GenerateDownloadLink(string upload_resopnse)
        {
            return string.Format(Constants.DOWNLOAD_URL, ExtractFileName(upload_resopnse));
        }

        private string ExtractFileName(string upload_resopnse)
        {
            return Regex.Match(upload_resopnse, "\\?action=download&f=(?<f>.*?)\"").Groups["f"].Value;
        }


        private string DownloadFile(string downloadUrl)
        {
            return Get(downloadUrl).Trim();
        }




        #region Request Utilities

        /// <summary>
        /// Sends a get request to the Url provided using this session cookies.
        /// </summary>
        /// <returns> The response of the request </returns>
        public string Get(string url)
        {
            using (var client = CreateHttpClient())
                return client.DownloadString(url);
        }

        /// <summary>
        /// Sends a get request to the Url provided using this session cookies.
        /// </summary>
        /// <returns> The response of the request </returns>
        public async Task<string> GetAsync(string url)
        {
            using (var client = CreateHttpClient())
                return await client.DownloadStringTaskAsync(url);
        }

        /// <summary>
        /// Sends a get request to the Url provided using this session cookies and returns the HtmlDocument of the result.
        /// </summary>
        public HtmlDocument GetHtmlDocument(string url)
        {
            using (var client = CreateHttpClient())
            {
                var response = client.DownloadString(url);

                var document = new HtmlDocument();
                document.LoadHtml(response);

                return document;
            }
        }

        /// <summary>
        /// Sends a get request to the Url provided using this session cookies.
        /// </summary>
        /// <returns> HtmlDocument of the result </returns>
        public async Task<HtmlDocument> GetHtmlDocumentAsync(string url)
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.DownloadStringTaskAsync(url);

                var document = new HtmlDocument();
                await Task.Run(() => document.LoadHtml(response));

                return document;
            }
        }

        /// <summary>
        /// Sends a post request to the Url provided using this session cookies and provided headers.
        /// </summary>
        /// <returns> The response of the request. </returns>
        public byte[] Post(string url, byte[] data, WebHeaderCollection headers, int timeout = 30000)
        {
            using (var client = CreateHttpClient())
            {
                client.Timeout = timeout;

                if (headers != null)
                    foreach (string header in headers)
                        client.Headers.Set(header, headers[header]);

                return client.UploadData(url, data);
            }
        }

        /// <summary>
        /// Sends a post request to the Url provided using this session cookies and provided headers.
        /// </summary>
        /// <returns> The response of the request. </returns>
        public async Task<byte[]> PostAsync(string url, byte[] data, WebHeaderCollection headers, int timeout = 30000)
        {
            using (var client = CreateHttpClient())
            {
                client.Timeout = timeout;

                if (headers != null)
                    foreach (string header in headers)
                        client.Headers.Set(header, headers[header]);

                return await client.UploadDataTaskAsync(url, data);
            }
        }

        /// <summary>
        /// Creates HttpClient that have this session cookies and default headers.
        /// </summary>
        private HttpClient CreateHttpClient(int timeout = 30000)
        {
            return new HttpClient()
            {
                DefaultHeaders = Constants.DefaultHeaders,
                Encoding = Encoding.UTF8,
                CookieContainer = cookies,
                Timeout = timeout
            };
        }

        #endregion

    }
}
