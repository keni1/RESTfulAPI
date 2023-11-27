using System.Text.Json.Serialization;

namespace RESTfulAPI.Models
{
    public class Story
    {
        [JsonPropertyName("title")]
        public string? Title { get; init; }

        [JsonPropertyName("url")]
        public string? Uri { get; init; }
        
        [JsonPropertyName("by")]
        public string? PostedBy { get; init; }
        
        [JsonPropertyName("time")]
        public long Time { get; init; } // Assuming this is Unix time format, then it will overflow a C# int in 2038
        
        [JsonPropertyName("score")]
        public int Score { get; init; }
        
        [JsonPropertyName("descendants")]
        public int CommentCount { get; init; }
    }
}
