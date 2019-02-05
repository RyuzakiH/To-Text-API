using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToText.API
{
    public class LanguagesLoader
    {
        private readonly ToTextClient client;

        public LanguagesLoader(ToTextClient client)
        {
            this.client = client;
        }

        public Dictionary<Languages, string> Load()
        {
            return GetAvailableLanguages()
                .Select(l => new KeyValuePair<Languages, string>(
                    (Languages)Enum.Parse(typeof(Languages), LanguageStringToEnumName(l.Key)), l.Value))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public async Task<Dictionary<Languages, string>> LoadAsync()
        {
            return await Task.Run(async () => (await GetAvailableLanguagesAsync())
                .Select(l => new KeyValuePair<Languages, string>(
                    (Languages)Enum.Parse(typeof(Languages), LanguageStringToEnumName(l.Key)), l.Value))
                .ToDictionary(x => x.Key, x => x.Value)
                );
        }


        private Dictionary<string, string> GetAvailableLanguages()
        {
            var document = client.GetHtmlDocument(Constants.BASE_URL);

            return ExtractLanguages(document);
        }

        private async Task<Dictionary<string, string>> GetAvailableLanguagesAsync()
        {
            var document = await client.GetHtmlDocumentAsync(Constants.BASE_URL);

            return ExtractLanguages(document);
        }

        // extracts keyvalue pairs <language_name, language_key>
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

    }
}
