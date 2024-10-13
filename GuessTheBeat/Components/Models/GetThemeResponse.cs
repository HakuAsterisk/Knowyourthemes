using System.Text.Json.Serialization;
namespace GuessTheBeat.Components.Models
{
    public class GetThemeResponse       //Model for handling the AnimeThemes API response
    {                                   //We collect a bit more information here than we use right now
        [JsonPropertyName("anime")]     //I've included it because I figured it might be useful later.
        public List<Anime>? Anime { get; set; }
    }

    public class Anime
    {

        [JsonPropertyName("name")]
        public string? Name { get; set; }   //Romanji name held on to so we have a name to fall back on if english is not available

        [JsonPropertyName("animethemes")]
        public List<AnimeThemes>? AnimeThemes { get; set; }

        [JsonPropertyName("animesynonyms")]
        public List<AnimeSynonyms>? Synonyms { get; set; }
    }

    public class AnimeSynonyms
    {

        [JsonPropertyName("text")]
        public string? Text { get; set; }   //Synonyms used for grabbing the english name of a show

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class AnimeThemes
    {
        [JsonPropertyName("slug")]
        public string? Slug { get; set; }   //Using the slug to get unique OP's and ED's

        [JsonPropertyName("animethemeentries")]
        public List<AnimeThemeEntries>? AnimeThemeEntries { get; set; } 
    }

    public class AnimeThemeEntries
    {
        [JsonPropertyName("videos")]
        public List<Video>? Videos { get; set; }
    }

    public class Video
    {
        [JsonPropertyName("link")]
        public string? Link { get; set; } //Link to OP/ED video

        [JsonPropertyName("audio")]
        public Audio? Audio { get; set; }
    }

    public class Audio
    {
        [JsonPropertyName("link")]
        public string? Link { get; set; }   //Link to OP/ED audio
    }
}