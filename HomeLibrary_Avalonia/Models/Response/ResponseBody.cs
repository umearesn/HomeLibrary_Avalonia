using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;
using Network;

namespace HomeLibrary_Avalonia.Models.Response
{
    public class ResponseBody<T>
    {

        [JsonPropertyName("status")]
        public string Status { get; set; }
        
        [JsonPropertyName("totalHits")]
        public int TotalHits { get; set; }
        
        [JsonPropertyName("data")]
        public List<T> Data { get; set; }
    }
}