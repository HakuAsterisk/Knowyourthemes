using System.Text.Json.Serialization;

namespace GuessTheBeat.Components.Models
{
    public class SearchResult
    {
        [JsonPropertyName("search")]
        public SearchInfo? Info { get; set; }
    }

    public class SearchInfo
    {
        [JsonPropertyName("anime")]
        public List<SearchData>? Data { get; set; }
    }

    public class SearchData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("season")]
        public string? Season { get; set; }

        [JsonPropertyName("media_format")]
        public string? MediaFormat { get; set; }

        [JsonPropertyName("synopsis")]
        public string? Synopsis { get; set; }
        
        [JsonPropertyName("animesynonyms")]
        public List<AnimeSynonym>? Synonyms { get; set; }
    }
    public class AnimeSynonym
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
