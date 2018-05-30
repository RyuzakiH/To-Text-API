using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ToText
{
    public class ToText
    {
        private static Dictionary<Languages, string> LanguagesKeys;

        private static bool AvailableLanguagesLoaded { get { return LanguagesKeys != null && LanguagesKeys.Count > 0; } }

        private CookieContainer Cookies;

        public ToText()
        {
            if (!AvailableLanguagesLoaded)
                MapAvailableLanguages();

            Cookies = new CookieContainer();
        }


        private void MapAvailableLanguages()
        {
            LanguagesKeys = GetAvailableLanguages()
                .Select(l => new KeyValuePair<Languages, string>(
                    (Languages)Enum.Parse(typeof(Languages), LanguageStringToEnumName(l.Key)), l.Value))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private Dictionary<string, string> GetAvailableLanguages()
        {
            var document = new HtmlDocument();

            string res = LoadMainPage();

            document.LoadHtml(res);

            return ExtractLanguages(document);
        }

        private string LoadMainPage()
        {
            return CreateHttpClient().DownloadString("http://www.to-text.net");
        }

        private Dictionary<string, string> ExtractLanguages(HtmlDocument document)
        {
            HtmlNode select = document.DocumentNode.Descendants().FirstOrDefault(n => n.GetAttributeValue("name", null) == "ocr_lang");
            return select.Descendants("option").Select(o => new KeyValuePair<string, string>(o.InnerText, o.GetAttributeValue("value", null)))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private static string LanguageStringToEnumName(string str)
        {
            return str.Trim().Replace(" ", "_").Replace("[", "").Replace("]", "");
        }



        public string Convert(Image image, Languages language)
        {
            return ConvertFile(MakeFile(image), language);
        }

        public string Convert(byte[] fileBytes, Languages language)
        {
            return ConvertFile(MakeFile(fileBytes), language);
        }

        public string Convert(string file_path, Languages language)
        {
            return ConvertFile(MakeFile(file_path), language);
        }

        private string ConvertFile(File file, Languages language)
        {
            var res = UploadFile("http://www.to-text.net/", file, LanguagesKeys[language]);

            if (CheckIfConverted(res))
                return DownloadFile(GenerateDownloadLink(res));

            return null;
        }



        private File MakeFile(string path)
        {
            var file = new File();
            file.Load(path);
            return file;
        }

        private File MakeFile(Image image)
        {
            var file = new File();
            file.Load(image);
            return file;
        }

        private File MakeFile(byte[] bytes)
        {
            var file = new File();
            file.Load(bytes);
            return file;
        }



        private string GenerateDownloadLink(string upload_resopnse)
        {
            return string.Format("http://www.to-text.net/?action=download&f={0}", ExtractFileName(upload_resopnse));
        }

        private string ExtractFileName(string upload_resopnse)
        {
            return Regex.Match(upload_resopnse, "\\?action=download&f=(?<f>.*?)\"").Groups["f"].Value;
        }

        private bool CheckIfConverted(string upload_resopnse)
        {
            return !upload_resopnse.Contains("Failed to convert");
        }


        
        private string UploadFile(string url, File file, string language_key)
        {
            using (var Client = CreateHttpClient())
            {
                var boundary = GenerateWebKitFormBoundary();

                AddUploadHeaders(Client, boundary);

                var data = BuildUploadData(boundary, language_key, file);

                return Encoding.ASCII.GetString(Client.UploadData(url, "POST", data));
            }
        }

        private string GenerateWebKitFormBoundary()
        {
            return DateTime.Now.Ticks.ToString("X");
        }

        private void AddUploadHeaders(HttpClient client, string boundary)
        {
            client.Headers.Set("Content-Type", "multipart/form-data; boundary=----WebKitFormBoundary" + boundary);
            client.Headers.Add("Referer", "http://www.to-text.net/");
        }

        private byte[] BuildUploadData(string boundary, string language_key, File file)
        {
            var data = Encoding.ASCII.GetBytes(string.Format("------WebKitFormBoundary{0}\r\nContent-Disposition: form-data; name=\"action\"\r\n\r\nconvert\r\n------WebKitFormBoundary{0}\r\nContent-Disposition: form-data; name=\"ocr_lang\"\r\n\r\n{1}\r\n------WebKitFormBoundary{0}\r\nContent-Disposition: form-data; name=\"doc_file\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n", boundary, language_key, file.Name, file.Type)).ToList();
            data.AddRange(file.Bytes);
            data.AddRange(Encoding.ASCII.GetBytes("\r\n------WebKitFormBoundary" + boundary));
            return data.ToArray();
        }



        private string DownloadFile(string downloadUrl)
        {
            return CreateHttpClient().DownloadString(downloadUrl).Trim();
        }



        private HttpClient CreateHttpClient()
        {
            return new HttpClient()
            {
                DefaultHeaders = new System.Net.WebHeaderCollection()
            {
                { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8" },
                { "Accept-Encoding", "gzip, deflate" },
                { "Accept-Language", "en-US,en" },
                { "User-Agent", "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36" },
                { "Origin", "http://www.to-text.net" },
                { "Host", "www.to-text.net" }
            },
                CookieContainer = Cookies
            };
        }


    }
}
