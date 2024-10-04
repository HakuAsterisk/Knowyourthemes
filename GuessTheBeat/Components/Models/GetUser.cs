using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace GuessTheBeat.Components.Models
{
    public class Title
    {
        [JsonPropertyName("romaji")]
        public string Romaji { get; set; }

        [JsonPropertyName("english")]
        public string English { get; set; }

        [JsonPropertyName("native")]
        public string Native { get; set; }
    }

    public class Media
    {
        [JsonPropertyName("title")]
        public Title Title { get; set; }
    }

    public class MediaList
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("media")]
        public Media Media { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class Page
    {
        [JsonPropertyName("mediaList")]
        public List<MediaList> MediaList { get; set; }
    }

    public class GetUser
    {
        [JsonPropertyName("Page")]
        public Page Page { get; set; }
    }
}

