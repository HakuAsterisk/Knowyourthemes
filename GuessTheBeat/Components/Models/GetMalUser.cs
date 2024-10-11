using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuessTheBeat.Components.Models{
    public class GetMalUser
    {
        [JsonPropertyName("data")]
        public List<AnimeData>? Data { get; set; }

        [JsonPropertyName("paging")]
        public Paging? Paging { get; set; }
    }
    // This is the data class for the API response
    public class AnimeData
    {

        [JsonPropertyName("node")]
        public AnimeNode? Node { get; set; }
    }
    public class AnimeNode
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("main_picture")]
        public MainPicture? MainPicture { get; set; }

        // Move media_type to this class
        [JsonPropertyName("media_type")]
        public string? MediaType { get; set; }
    }

    // Model for the "main_picture" object with image URLs
    public class MainPicture
    {
        [JsonPropertyName("medium")]
        public string? Medium { get; set; }

        [JsonPropertyName("large")]
        public string? Large { get; set; }
    }

    // Model for the "paging" object that contains pagination information
    public class Paging
    {
        [JsonPropertyName("next")]
        public string? Next { get; set; }
    }
}