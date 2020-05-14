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

        [JsonPropertyName("citations")]
        public List<Citation> Citations { get; set; }
        
        [JsonPropertyName("contributors")]
        public List<string> Contributors { get; set; }

        [JsonPropertyName("datePublished")]
        public string DatePublished { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("fullText")]
        public string Fulltext { get; set; }
        
        [JsonPropertyName("identifiers")]
        public List<string> Identifiers { get; set; }
        
        [JsonPropertyName("journals")]
        public List<JournalObject> Journals { get; set; }
        
        [JsonPropertyName("language")]
        public LanguageObject Language { get; set; }
        
        [JsonPropertyName("publisher")]
        public string Publisher { get; set; }
        
        [JsonPropertyName("relations")]
        public List<string> RelatedUrl { get; set; }
        
        /// <summary>
        ///  List of repositories this article belongs to.
        /// </summary>
        [JsonPropertyName("repositories")]
        public List<RepoObject> SourceRepositories { get; set; }
        
        /// <summary>
        ///  Information of CORE harvesting.
        /// </summary>
        [JsonPropertyName("repositoryDocument")]
        public RepoDoc RepoDoc { get; set; }
        
        [JsonPropertyName("similarities")]
        public List<Similar> Similarity { get; set; }
        
        [JsonPropertyName("subjects")]
        public List<string> Subjects { get; set; }
        
        [JsonPropertyName("types")]
        public List<string> Types { get; set; }
        
        [JsonPropertyName("year")]
        public int Year { get; set; }
        
        // IDK what's the difference of the two properties below, it's is given in the manual.
        
        /// <summary>
        /// URLs of the fulltext version of this article.
        /// </summary>
        [JsonPropertyName("fulltextUrls")]
        public List<string> FulltextUrls { get; set; }
        
        /// <summary>
        ///  The URL to the fulltext.
        /// </summary>
        [JsonPropertyName("fulltextIdentifier")]
        public string FulltextIndentifier { get; set; }
        
        [JsonPropertyName("doi")]
        public string Doi { get; set; }
        
        [JsonPropertyName("oai")]
        public string Oai { get; set; }
        
        /// <summary>
        /// The download PDF URL which is displayed on /display/[ArticleID] page.
        /// </summary>
        [JsonPropertyName("downloadUrl")]
        public string DownloadUrl { get; set; }

        [JsonPropertyName("pdfPath")]
        public string PdfPath { get; set; }
    }
    
    public class Citation
    {
        [JsonPropertyName("raw")]
        public string RawText { get; set; }
        
        [JsonPropertyName("authors")]
        public List<string> Authors { get; set; }
        
        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("date")]
        public string PublishingDate { get; set; }
        
        [JsonPropertyName("doi")]
        public string Doi { get; set; }
    }
    
    public class LanguageObject
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Similar
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("score")]
        public double Score { get; set; }
    }
}