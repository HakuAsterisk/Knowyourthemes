using System.Text.Json.Serialization;
namespace GuessTheBeat.Components.Models
{
    public class GetAniUser //Model to handle the AniList API responses. Built to handle both the popular list
    {                       //API response and individual users AniList API response.
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
        public PageInfo? PageInfo { get; set; } //PageInfo for pagination

        [JsonPropertyName("media")]
        public List<Media>? Media { get; set; } //Popular anime converges into media here 
                                                //whereas user animelist needs the medialist property below
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
        public Media? Media { get; set; }       //For handling a users medialist
    }

    public class Media
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("format")]            //A specific users medialist goes into Media here
        public string? Format { get; set; }     //Both use the same class but move into it from different points
    }
}
