using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuessTheBeat.Components.Models
{
    // Root object for the API response
    public class GetAniList
    {
        [JsonPropertyName("anime")]
        public List<AniListAnime?> Anime { get; set; }
    }

    // Represents each anime in the anime list
    public class AniListAnime
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }                   // AnimeThemes ID

        [JsonPropertyName("name")]
        public string? Name { get; set; }              // Anime name

        [JsonPropertyName("media_format")]
        public string? MediaFormat { get; set; }       // Media format (e.g., TV, Movie)

        [JsonPropertyName("season")]
        public string? Season { get; set; }            // Season (e.g., Fall)

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }              // URL-friendly slug

        [JsonPropertyName("synopsis")]
        public string? Synopsis { get; set; }          // Short description or plot summary

        [JsonPropertyName("year")]
        public int Year { get; set; }                 // Release year

        [JsonPropertyName("animethemes")]
        public List<AnimeTheme>? AnimeThemes { get; set; } // List of themes like OP/ED
    }

    // Represents each theme (e.g., Opening/Ending themes)
    public class AnimeTheme
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }                   // Theme ID

        [JsonPropertyName("sequence")]
        public int? Sequence { get; set; }            // Sequence of the theme (e.g., OP1, ED1)

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }              // Slug for the theme (e.g., OP1, ED1)

        [JsonPropertyName("type")]
        public string? Type { get; set; }              // Type of theme (OP/ED)

        [JsonPropertyName("animethemeentries")]
        public List<AnimeThemeEntry>? AnimeThemeEntries { get; set; } // Entries within the theme
    }

    // Represents each entry within a theme
    public class AnimeThemeEntry
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }                   // Theme entry ID

        [JsonPropertyName("episodes")]
        public string? Episodes { get; set; }          // Episodes where this theme is used

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }             // Additional notes

        [JsonPropertyName("nsfw")]
        public bool? IsNsfw { get; set; }              // Is NSFW

        [JsonPropertyName("spoiler")]
        public bool? IsSpoiler { get; set; }           // Is it a spoiler?

        [JsonPropertyName("version")]
        public int? Version { get; set; }             // Version of the theme entry

        [JsonPropertyName("videos")]
        public List<ThemeVideo>? Videos { get; set; }  // List of related videos
    }

    // Represents the video object within a theme entry
    public class ThemeVideo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }                   // Video ID

        [JsonPropertyName("basename")]
        public string? BaseName { get; set; }          // Base name of the video

        [JsonPropertyName("filename")]
        public string? FileName { get; set; }          // File name of the video

        [JsonPropertyName("lyrics")]
        public bool HasLyrics { get; set; }           // Does the video have lyrics?

        [JsonPropertyName("nc")]
        public bool IsNc { get; set; }                // No credits (NC)

        [JsonPropertyName("overlap")]
        public string? Overlap { get; set; }           // Overlap type (None, Transition, etc.)

        [JsonPropertyName("path")]
        public string? Path { get; set; }              // Path to the video

        [JsonPropertyName("resolution")]
        public int Resolution { get; set; }           // Video resolution (e.g., 720, 1080)

        [JsonPropertyName("size")]
        public long Size { get; set; }                // Size of the video file

        [JsonPropertyName("source")]
        public string? Source { get; set; }            // Source of the video (e.g., WEB, BD)

        [JsonPropertyName("subbed")]
        public bool IsSubbed { get; set; }            // Is it subbed?

        [JsonPropertyName("uncen")]
        public bool IsUncensored { get; set; }        // Is it uncensored?

        [JsonPropertyName("tags")]
        public string? Tags { get; set; }              // Tags for the video

        [JsonPropertyName("link")]
        public string? Link { get; set; }              // Link to the video

        [JsonPropertyName("audio")]
        public ThemeAudio? Audio { get; set; }         // Audio information for the video
    }

    // Represents the audio object within a video
    public class ThemeAudio
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }                   // Audio ID

        [JsonPropertyName("basename")]
        public string? BaseName { get; set; }          // Base name of the audio file

        [JsonPropertyName("filename")]
        public string? FileName { get; set; }          // File name of the audio file

        [JsonPropertyName("path")]
        public string? Path { get; set; }              // Path to the audio file

        [JsonPropertyName("size")]
        public long Size { get; set; }                // Size of the audio file

        [JsonPropertyName("link")]
        public string? Link { get; set; }              // Link to the audio file
    }
}
