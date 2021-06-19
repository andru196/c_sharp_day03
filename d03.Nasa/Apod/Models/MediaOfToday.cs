using System;
using System.Text.Json.Serialization;

namespace d03.Nasa.Apod.Models
{
    public record MediaOfToday
    {
        
        [JsonPropertyName("date")]
        public DateTime DateTime{get; init;}
        
        [JsonPropertyName("copyright")]
        public string Copyright {get;init;}
        [JsonPropertyName("explanation")]
        public string Explanation {get; init;}
        [JsonPropertyName("hdurl")]
        public string Hdurl {get; init;}
        [JsonPropertyName("media_type")]
        public string MediaType {get; init;}
        [JsonPropertyName("title")]
        public string Title {get; init;}
        [JsonPropertyName("url")]
        public string Url {get; init;}

        public override string ToString()
        =>
        $"{DateTime}\n"+
        $"‘{Title}’ by {Copyright}\n" +
        $"{Explanation}\n" +
        $"{Url}";
    }
}