using System.Text.Json.Serialization;

namespace GuessTheBeat.Components.Models
{
    public class AnimeImage
    {
        [JsonPropertyName("image")]
        public ImageDetails ImageDetails { get; set; }
    }

    public class ImageDetails
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("mimetype")]
        public string Mimetype { get; set; }

        [JsonPropertyName("facet")]
        public string Facet { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonPropertyName("deleted_at")]
        public string DeletedAt { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }
    }
}
