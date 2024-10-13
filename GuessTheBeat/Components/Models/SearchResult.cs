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
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        
        [JsonPropertyName("animesynonyms")]
        public List<AnimeSynonym>? Synonyms { get; set; }   //Name to help find the right show and to have something to use if eng isn's available
    }
    public class AnimeSynonym
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }   //Synonyms to fetch the possible english name

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
