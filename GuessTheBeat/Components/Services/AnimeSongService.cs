// Components/Services/AnimeSongService.cs
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using GuessTheBeat.Components.Models;
using System.Text.RegularExpressions;

namespace GuessTheBeat.Components.Services
{
    public class AnimeInfo
    {
    public int Id { get; set; }
    public string? RomajiTitle { get; set; }
    public string? EnglishTitle { get; set; }
    public string? NativeTitle { get; set; }
    }
    public class PopularInfo
    {
    public int Id { get; set; }
    public string? RomajiTitle { get; set; }
    public string? EnglishTitle { get; set; }
    public string? NativeTitle { get; set; }
    }

    public class AnimeResult
    {
        public int AnimeId { get; set; }
        public string? AnimeSlug { get; set; }
        public string? AnimeName { get; set; }
        public int ThemeId { get; set; }
        public string? ThemeSlug { get; set; }
        public int ThemeEntryId { get; set; }
        public string? VideoLink { get; set; }
        public string? AudioLink { get; set; }
    }

    public class FoundResult
    {
        public string? ResultName { get; set; }
    }

    public class AnimeSongService
    {
        private readonly HttpClient _httpClient;
        private static readonly Random _random = new Random();

        public AnimeSongService(HttpClient httpClient)
        {
        _httpClient = httpClient;
        }

        public async Task<GetAnime> GetAnimeInfoAsync(string slug)
        {
            var response = await _httpClient.GetAsync($"https://api.animethemes.moe/anime/{slug}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetAnime>();
        }
        
//----------------------------------MYANIMELIST-----------------------------

    public async Task<List<AnimeNode>> GetMediaListAsync(string query)
    {
    // Build the URL
    var malUrl = "https://api.myanimelist.net/v2/users/" + query + "/animelist?limit=50&status=completed&fields=media_type";

    var request = new HttpRequestMessage(HttpMethod.Get, malUrl);
    request.Headers.Add("X-MAL-CLIENT-ID", "f1310d1d225d6ec4f0107e341a2788ad");

    var response = await _httpClient.SendAsync(request);
    response.EnsureSuccessStatusCode();

    var responseContent = await response.Content.ReadAsStringAsync();
    var root = JsonSerializer.Deserialize<GetMalUser>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    // Filter the anime list to only include items with MediaType "tv"
    var animeList = root.Data
        .Where(anime => anime.Node.MediaType.Equals("tv", StringComparison.OrdinalIgnoreCase)) // Filter for media type "tv"
        .Select(anime => new AnimeNode
        {
            Id = anime.Node.Id,
            Title = anime.Node.Title,
            MainPicture = anime.Node.MainPicture // Include main picture if needed
        })
        .ToList();

    // Shuffle and return 10 unique titles
    var randomizedList = animeList
        .OrderBy(x => _random.Next()) // Shuffle
        .DistinctBy(x => x.Id)        // Ensure uniqueness
        .Take(10)                     // Take first 10
        .ToList();

    return randomizedList;
    }

//--------------------ANILIST--------------------------
    public async Task<List<PopularInfo>> GetPopularAsync()
    {
        var query = @"
        query{
            Page(page: 1, perPage: 50) {
                media(sort: [POPULARITY_DESC, FORMAT_DESC], format_in: [TV]) {
                id
                format
                title {
                    romaji
                    english
                    native
                }
                }
            }
            }";

        var requestBody = new
        {
            query = query,
            variables = new { }  // Empty object since we are not using variables
        };

        // Convert the request body to JSON
        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Set the request URL
        var url = "https://graphql.anilist.co";

        // Send the POST request
        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode(); // Throw if not a success code.

        // Read the response content
        var responseContent = await response.Content.ReadAsStringAsync();

        // Deserialize the response into the GetAniUser model
        var aniUser = JsonSerializer.Deserialize<GetPopular>(responseContent);

        List<PopularInfo> popularInfoList = new List<PopularInfo>();
        
        if (aniUser?.Data?.Page?.Media != null)
        {
            foreach (var mediaItem in aniUser.Data.Page.Media)
            {
                if(mediaItem.Format == "TV"){
                var animeInfo = new PopularInfo
                {
                    Id = mediaItem.Id,
                };

                popularInfoList.Add(animeInfo);
                }
            }
        }
        var randomizedList = popularInfoList
                                .OrderBy(x => _random.Next())  // Shuffle
                                .DistinctBy(x => x.Id) // Ensure uniqueness
                                .Take(10)                     // Take first 100
                                .ToList();
        return randomizedList; // Return the list of AnimeInfo objects
    }

    private int currentPage = 1;
    private int perPage = 50;
    private bool hasNextPage = true;

    public async Task<List<AnimeInfo>> GetAniListAsync(string username)
    {

        List<AnimeInfo> animeInfoList = new List<AnimeInfo>();

        while (hasNextPage)
        {
            // Define the GraphQL query with variables for pagination
            var query = $@"
            query {{
                Page(page: {currentPage}, perPage: {perPage}) {{
                    pageInfo {{
                        hasNextPage
                    }}
                    mediaList(userName: ""{username}"", sort: SCORE_DESC, status: COMPLETED) {{
                        media {{
                            id
                            format
                            title {{
                                romaji
                                english
                                native
                            }}
                        }}
                        score
                        status
                    }}
                }}
            }}";

            var requestBody = new
            {
                query = query
            };

            // Convert the request body to JSON
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Set the request URL
            var url = "https://graphql.anilist.co";

            // Send the POST request
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode(); // Throw if not a success code.

            // Read the response content
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize the response into the GetAniUser model
            var aniUser = JsonSerializer.Deserialize<GetAniUser>(responseContent);

            if (aniUser?.Data?.Page?.MediaList != null)
            {
                foreach (var mediaItem in aniUser.Data.Page.MediaList)
                {
                    // Filter only TV format shows
                    if (mediaItem.Media.Format == "TV")
                    {
                        var animeInfo = new AnimeInfo
                        {
                            Id = mediaItem.Media.Id,
                            // You can add more properties here if needed
                        };

                        animeInfoList.Add(animeInfo);
                    }
                }

                // Check if there's another page to fetch
                hasNextPage = aniUser.Data.Page.PageInfo.HasNextPage;
            }
            else
            {
                hasNextPage = false; // Stop if no data
            }

            currentPage++; // Move to the next page
        }
        // Randomize the list, ensure uniqueness, and take 10
        var randomizedList = animeInfoList
                            .OrderBy(x => _random.Next())  // Shuffle
                            .DistinctBy(x => x.Id)         // Ensure uniqueness
                            .Take(10)                      // Take first 10
                            .ToList();
        return randomizedList;
    }

        //---------------------------- FETCH SONG---------------------------

        public async Task<List<AnimeResult>> ConvertIds(int oldId, string site)
        {
            // Base URL for the AnimeThemes API
            string apiUrl = "https://api.animethemes.moe/anime?filter[has]=resources&filter[site]=";

            // Select the correct site filter (MyAnimeList or AniList)
            if (site == "mal")
            {
                apiUrl += "MyAnimeList&filter[external_id]=" + oldId + "&include=animethemes.animethemeentries.videos.audio,animesynonyms";
            }
            else if (site == "ani")
            {
                apiUrl += "AniList&filter[external_id]=" + oldId + "&include=animethemes.animethemeentries.videos.audio,animesynonyms";
            }
            else
            {
                throw new ArgumentException("Invalid site identifier. Must be 'mal' or 'ani'.");
            }

            // Call the API and ensure success
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into the root object
            var root = JsonSerializer.Deserialize<GetThemeResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var animeResults = new List<AnimeResult>();

            // Regex to match OP# and ED#
            var opEdRegex = new Regex(@"^(OP|ED)\d+$", RegexOptions.IgnoreCase);

            // Set to keep track of slugs we have already processed
            var slugSeen = new HashSet<string>();

            // Ensure we have anime data
            if (root?.Anime != null && root.Anime.Any())
            {
                var anime = root.Anime.FirstOrDefault();

                if (anime != null)
                {
                    // Attempt to find the English synonym, if it exists
                    var englishSynonym = anime.Synonyms?
                        .FirstOrDefault(s => s.Type != null && s.Type.Equals("English", StringComparison.OrdinalIgnoreCase));

                    // Fallback to the original anime name if no English synonym exists
                    string animeName = englishSynonym?.Text ?? anime.Name;

                    if (anime.AnimeThemes != null)
                    {
                        foreach (var theme in anime.AnimeThemes)
                        {
                            // Check if the theme slug matches "OP#" or "ED#" and has not been seen before
                            if (theme.Slug != null && opEdRegex.IsMatch(theme.Slug) && !slugSeen.Contains(theme.Slug))
                            {
                                // Add the theme slug to the seen set to avoid duplicates
                                slugSeen.Add(theme.Slug);

                                if (theme.AnimeThemeEntries != null)
                                {
                                    foreach (var entry in theme.AnimeThemeEntries)
                                    {
                                        if (entry.Videos != null)
                                        {
                                            foreach (var video in entry.Videos)
                                            {
                                                // Create the AnimeResult object with the English name or fallback name
                                                var animeResult = new AnimeResult
                                                {
                                                    AnimeId = anime.Id,
                                                    AnimeSlug = anime.Slug,
                                                    AnimeName = animeName, // Use the English synonym if available
                                                    ThemeId = theme.Id,
                                                    ThemeSlug = theme.Slug,
                                                    ThemeEntryId = entry.Id,
                                                    VideoLink = video.Link,
                                                    AudioLink = video.Audio?.Link // Use null conditional operator to avoid null reference
                                                };

                                                // Add this result to the list
                                                animeResults.Add(animeResult);

                                                // Exit the loop once the first version of OP/ED has been added
                                                break;
                                            }

                                            // Exit once we've handled the first entry for this theme
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return animeResults;
        }

        public async Task<List<FoundResult>> SearchAnimeAsync(string query)
        {
            // Replace with your actual API endpoint
            var response = await _httpClient.GetFromJsonAsync<SearchResult>($"https://api.animethemes.moe/search?fields[search]=anime&q={query}&include[anime]=animesynonyms");

            // Check if response and its properties are not null
            var foundResults = new List<FoundResult>();

            // Ensure Info and Data are not null
            if (response?.Info?.Data != null)
            {
                // Loop through each anime entry
                foreach (var data in response.Info.Data)
                {
                    // Attempt to find the English synonym
                    var englishSynonym = data.Synonyms?.FirstOrDefault(s => s.Type != null && s.Type.Equals("English", StringComparison.OrdinalIgnoreCase));

                    // Fallback to the original anime name if no English synonym exists
                    string animeName = englishSynonym?.Text ?? data.Name;

                    var foundResult = new FoundResult
                    {
                        ResultName = animeName
                    };

                    foundResults.Add(foundResult);
                }
            }

            // Return the list of found results
            return foundResults;
        }
    }
}

