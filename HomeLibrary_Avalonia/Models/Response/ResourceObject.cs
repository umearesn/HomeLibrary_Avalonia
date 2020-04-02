using System.Text.Json;
using System.Text.Json.Serialization;
using Nest;

namespace HomeLibrary_Avalonia.Models.Response
{

    public class ResourceObject
    {
        [JsonPropertyName("_type")]
        public string Type { get; set; }
        
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        
        [JsonPropertyName("_score")]
        public double Score { get; set; }
        
        [JsonPropertyName("_source")]
        public JsonElement Source { get; set; }
    }
}