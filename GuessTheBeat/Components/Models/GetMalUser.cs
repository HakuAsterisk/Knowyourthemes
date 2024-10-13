using System.Text.Json.Serialization;
namespace GuessTheBeat.Components.Models{
    public class GetMalUser //Model to handle the MAL API response
    {
        [JsonPropertyName("data")]
        public List<AnimeData>? Data { get; set; }

        [JsonPropertyName("paging")]
        public Paging? Paging { get; set; } //Paging option for looping through the search results
    }
    public class AnimeData
    {
        [JsonPropertyName("node")]
        public AnimeNode? Node { get; set; }
    }
    public class AnimeNode  //Model that contains MediaType so we can only pull tv-shows as movies and ONA's dont always have openings
    {                       //And ID's that we can use to reliably identify the show with AnimeThemes API
        [JsonPropertyName("id")]
        public int Id { get; set; }

        // Move media_type to this class
        [JsonPropertyName("media_type")]
        public string? MediaType { get; set; }
    }
    public class Paging
    {
        [JsonPropertyName("next")]
        public string? Next { get; set; }
    }
}