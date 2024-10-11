using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuessTheBeat.Components.Models
{
    public class GetThemeResponse
    {
        [JsonPropertyName("anime")]
        public List<Anime>? Anime { get; set; }

        [JsonPropertyName("links")]
        public Links? Links { get; set; }

        [JsonPropertyName("meta")]
        public Meta? Meta { get; set; }
    }

    public class Anime
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("media_format")]
        public string? MediaFormat { get; set; }

        [JsonPropertyName("season")]
        public string? Season { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("synopsis")]
        public string? Synopsis { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("animethemes")]
        public List<AnimeThemes>? AnimeThemes { get; set; }

        [JsonPropertyName("animesynonyms")]
        public List<AnimeSynonyms>? Synonyms { get; set; }
    }

    public class AnimeSynonyms
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class AnimeThemes
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sequence")]
        public int? Sequence { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("animethemeentries")]
        public List<AnimeThemeEntries>? AnimeThemeEntries { get; set; }
    }

    public class AnimeThemeEntries
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("episodes")]
        public string? Episodes { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }

        [JsonPropertyName("nsfw")]
        public bool Nsfw { get; set; }

        [JsonPropertyName("spoiler")]
        public bool Spoiler { get; set; }

        [JsonPropertyName("version")]
        public int? Version { get; set; }

        [JsonPropertyName("videos")]
        public List<Video>? Videos { get; set; }
    }

    public class Video
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("basename")]
        public string? BaseName { get; set; }

        [JsonPropertyName("filename")]
        public string? FileName { get; set; }

        [JsonPropertyName("lyrics")]
        public bool Lyrics { get; set; }

        [JsonPropertyName("nc")]
        public bool Nc { get; set; }

        [JsonPropertyName("overlap")]
        public string? Overlap { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("resolution")]
        public int Resolution { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("source")]
        public string? Source { get; set; }

        [JsonPropertyName("subbed")]
        public bool Subbed { get; set; }

        [JsonPropertyName("uncen")]
        public bool Uncen { get; set; }

        [JsonPropertyName("tags")]
        public string? Tags { get; set; }

        [JsonPropertyName("link")]
        public string? Link { get; set; }

        [JsonPropertyName("audio")]
        public Audio? Audio { get; set; }
    }

    public class Audio
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("basename")]
        public string? BaseName { get; set; }

        [JsonPropertyName("filename")]
        public string? FileName { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("link")]
        public string? Link { get; set; }
    }

    public class Links
    {
        [JsonPropertyName("first")]
        public string? First { get; set; }

        [JsonPropertyName("last")]
        public string? Last { get; set; }

        [JsonPropertyName("prev")]
        public string? Prev { get; set; }

        [JsonPropertyName("next")]
        public string? Next { get; set; }
    }

    public class Meta
    {
        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("from")]
        public int From { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("per_page")]
        public int PerPage { get; set; }

        [JsonPropertyName("to")]
        public int To { get; set; }
    }
}
