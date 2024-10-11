using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuessTheBeat.Components.Models
{
    public class GetAniUser
    {
        [JsonPropertyName("data")]
        public AniListData? Data { get; set; }
    }

    public class AniListData
    {
        [JsonPropertyName("Page")]
        public Page? Page { get; set; }
    }

    public class Page
    {
        [JsonPropertyName("pageInfo")]
        public PageInfo? PageInfo { get; set; }

        [JsonPropertyName("mediaList")]
        public List<MediaListItem>? MediaList { get; set; }
    }

    public class PageInfo
    {
        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }
    }

    public class MediaListItem
    {
        [JsonPropertyName("media")]
        public Media? Media { get; set; }

        [JsonPropertyName("score")]
        public float? Score { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

    public class Media
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("format")]
        public string? Format { get; set; }

        [JsonPropertyName("title")]
        public MediaTitle? Title { get; set; }
    }

    public class MediaTitle
    {
        [JsonPropertyName("romaji")]
        public string? Romaji { get; set; }

        [JsonPropertyName("english")]
        public string? English { get; set; }

        [JsonPropertyName("native")]
        public string? Native { get; set; }
    }
}
