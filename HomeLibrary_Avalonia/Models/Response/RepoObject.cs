using System;
using System.Text.Json.Serialization;

namespace HomeLibrary_Avalonia.Models.Response
{
    public class RepoObject
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } //they lied about int

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("urlOaipmh")]
        public string OaiPmh { get; set; }

        [JsonPropertyName("urlHomepage")]
        public string Homepage { get; set; }

        [JsonPropertyName("openDoarId")]
        public int DoarId { get; set; }

        [JsonPropertyName("repositoryLocation")]
        public RepoLocation Location { get; set; }

        [JsonPropertyName("repositoryStats")]
        public RepoStats Stats { get; set; }

        [JsonPropertyName("lastSeen")]
        public DateTime LastSeen { get; set; }
    }

    public class RepoLocation
    {
        [JsonPropertyName("id")]
        public int CoreRepoId { get; set; }

        [JsonPropertyName("repositoryName")]
        public string RepoName { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("latitude")]
        public int Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public int Longitude { get; set; }

        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
    }

    public class RepoStats
    {
        [JsonPropertyName("dateLastProcessed")]
        public string DateLastProcessed { get; set; }

        [JsonPropertyName("countMetadata")]
        public int CountMetadata { get; set; }

        [JsonPropertyName("countFulltext")]
        public int CountFulltext { get; set; }
    }

    // Throw away annotated - thinking
    public class RepoDoc
    {
        // May be important, but type is incorrect. Again
        //0 - allowed, 1 - deleted, 2 - disabled
        [JsonPropertyName("deletedStatus")]
        public string DeletedStatus { get; set; }

        [JsonPropertyName("indexed")]
        public int? Indexed { get; set; }

        // Throw away
        [JsonPropertyName("metadataUpdated")]
        public long? MetadataUpdated { get; set; }

        //Throw away
        [JsonPropertyName("depositedDate")]
        public long? DepositedDate { get; set; }

        // size in bytes
        [JsonPropertyName("pdfSize")]
        public long? PdfSize { get; set; }

        [JsonPropertyName("pdfStatus")]
        public int? PdfStatus { get; set; }

        [JsonPropertyName("textStatus")]
        public int? TextStatus { get; set; }

        // Throw  away
        [JsonPropertyName("timestamp")]
        public long? Timestamp { get; set; }

        // Throw away - fortunately, type is not missed
        [JsonPropertyName("tdmOnly")]
        public bool? TdmOnly { get; set; }

        [JsonPropertyName("pdfOrigin")]
        public string PdfOrigin { get; set; }


    }

}