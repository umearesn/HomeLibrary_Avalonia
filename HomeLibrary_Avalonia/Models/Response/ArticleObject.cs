using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HomeLibrary_Avalonia.Models.Response
{
    public class ArticleObject
    {

        [JsonPropertyName("id")]
        public string Id { get; set; } // документация врет, там стринга

        [JsonPropertyName("title")]
        public string Title { get; set; } = "default title";

        [JsonPropertyName("topics")]
        public List<string> Topics { get; set; }

        [JsonPropertyName("authors")]
        public List<string> Authors { get; set; }

        [JsonPropertyName("datePublished")]
        public string DatePublished { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("fullText")]
        public string Fulltext { get; set; }

        [JsonPropertyName("journals")]
        public List<JournalObject> Journals { get; set; }

        /// <summary>
        ///  The URL to the fulltext.
        /// </summary>
        [JsonPropertyName("fulltextIdentifier")]
        public string FulltextIndentifier { get; set; }

        /// <summary>
        /// The download PDF URL which is displayed on /display/[ArticleID] page.
        /// </summary>
        [JsonPropertyName("downloadUrl")]
        public string DownloadUrl { get; set; }

        [JsonPropertyName("pdfPath")]
        public string PdfPath { get; set; }
    }
}