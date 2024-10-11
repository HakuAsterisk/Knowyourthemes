using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuessTheBeat.Components.Models
{
    public class GetPopular
    {
        [JsonPropertyName("data")]
        public AniListPageData? Data { get; set; }
    }

    public class AniListPageData
    {
        [JsonPropertyName("Page")]
        public AniListPage? Page { get; set; }
    }

    public class AniListPage
    {
        [JsonPropertyName("media")]
        public List<MediaData>? Media { get; set; }
    }

    public class MediaData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("format")]
        public string? Format { get; set; }

        [JsonPropertyName("title")]
        public MediaTitles? Title { get; set; }
    }

    public class MediaTitles
    {
        [JsonPropertyName("romaji")]
        public string? Romaji { get; set; }

        [JsonPropertyName("english")]
        public string? English { get; set; }

        [JsonPropertyName("native")]
        public string? Native { get; set; }
    }
}
