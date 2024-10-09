using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuessTheBeat.Components.Models
{
    public class GetAniList
    {
        public List<AniListAnime> Anime { get; set; } // List of anime objects
    }

    public class AniListAnime
    {
        public int Id { get; set; }                   // AnimeThemes ID
        public string Name { get; set; }              // Anime name
        public string MediaFormat { get; set; }       // Media format (e.g., TV, Movie)
        public string Season { get; set; }            // Season (e.g., Fall)
        public string Slug { get; set; }              // URL-friendly slug
        public string Synopsis { get; set; }          // Short description or plot summary
        public int Year { get; set; }                 // Release year
    }
}
