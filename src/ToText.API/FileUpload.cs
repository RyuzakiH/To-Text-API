using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ToText.API
{
    public class FileUpload
    {
        private readonly ToTextClient client;

        public FileUpload(ToTextClient client)
        {
            this.client = client;
        }


        public string Upload(File file, string language_key)
        {
            var boundary = GenerateWebKitFormBoundary();

            var headers = MakeUploadHeaders(boundary);

            var data = BuildUploadData(boundary, language_key, file);

            var result = client.Post($"{Constants.BASE_URL}/", data, headers, 300000);

            return Encoding.ASCII.GetString(result);
        }

        public async Task<string> UploadAsync(File file, string language_key)
        {
            var result = await Task.Run(async () =>
            {
                var boundary = GenerateWebKitFormBoundary();

                var headers = MakeUploadHeaders(boundary);

                var data = BuildUploadData(boundary, language_key, file);

                return await client.PostAsync($"{Constants.BASE_URL}/", data, headers, 300000);
            });

            return Encoding.ASCII.GetString(result);
        }


        private string GenerateWebKitFormBoundary()
        {
            return DateTime.Now.Ticks.ToString("X");
        }

        private WebHeaderCollection MakeUploadHeaders(string boundary)
        {
            return new WebHeaderCollection
            {
                { "Content-Type", "multipart/form-data; boundary=----WebKitFormBoundary" + boundary },
                { "Referer", $"{Constants.BASE_URL}/" }
            };
        }

        private byte[] BuildUploadData(string boundary, string language_key, File file)
        {
            var data = Encoding.ASCII.GetBytes(string.Format(
                "------WebKitFormBoundary{0}\r\nContent-Disposition: form-data; name=\"action\"\r\n\r\nconvert\r\n------WebKitFormBoundary{0}\r\nContent-Disposition: form-data; name=\"ocr_lang\"\r\n\r\n{1}\r\n------WebKitFormBoundary{0}\r\nContent-Disposition: form-data; name=\"doc_file\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                boundary, language_key, file.Name, file.Type)).ToList();
            data.AddRange(file.Bytes);
            data.AddRange(Encoding.ASCII.GetBytes("\r\n------WebKitFormBoundary" + boundary));
            return data.ToArray();
        }

    }
}
