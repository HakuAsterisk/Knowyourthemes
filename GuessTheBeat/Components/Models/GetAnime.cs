using System.Text.Json.Serialization;

namespace GuessTheBeat.Components.Models
{
    public class GetAnime
    {
        [JsonPropertyName("anime")]
        public AnimeInfo AnimeInfo { get; set; }
    }

    public class AnimeInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("season")]
        public string Season { get; set; }

        [JsonPropertyName("media_format")]
        public string MediaFormat { get; set; }

        [JsonPropertyName("synopsis")]
        public string Synopsis { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonPropertyName("deleted_at")]
        public string DeletedAt { get; set; }
    }
}
