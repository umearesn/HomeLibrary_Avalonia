using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HomeLibrary_Avalonia.Models.Response
{
    public class JournalObject
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("identifiers")]
        public List<string> Identifiers { get; set; }
    }
}