// Components/Services/AnimeSongService.cs
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using GuessTheBeat.Components.Models;

namespace GuessTheBeat.Components.Services
{
    public class AnimeInfo
    {
    public int Id { get; set; }
    public string RomajiTitle { get; set; }
    public string EnglishTitle { get; set; }
    public string NativeTitle { get; set; }
    }

    public class AnimeSongService
    {
        private readonly HttpClient _httpClient;

        public AnimeSongService(HttpClient httpClient)
        {
        _httpClient = httpClient;
        }

        public async Task<AnimeImage> GetAnimeImageAsync(int imageId)
        {
            var response = await _httpClient.GetAsync($"https://api.animethemes.moe/image/{imageId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AnimeImage>();
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
        var malUrl = "https://api.myanimelist.net/v2/users/" + query + "/animelist?limit=50&status=completed";
        
        var request = new HttpRequestMessage(HttpMethod.Get, malUrl);
        request.Headers.Add("X-MAL-CLIENT-ID", "f1310d1d225d6ec4f0107e341a2788ad");
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var root = JsonSerializer.Deserialize<GetMalUser>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        // Map the API data to your view model (DataList)
        return root.Data.Select(anime => new AnimeNode
        {
            Id = anime.Node.Id,
            Title = anime.Node.Title
        }).ToList();
        }

//--------------------ANILIST--------------------------

        public async Task<List<AnimeInfo>> GetAniListAsync(string username)
        {
            var query = @"
            query {
                Page(page: 1, perPage: 50) {
                    mediaList(userName: ""HakuAsterisk"", sort: SCORE_DESC, status: COMPLETED) {
                        id
                        media {
                            title {
                                romaji
                                english
                                native
                            }
                        }
                        score
                        status
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
            var aniUser = JsonSerializer.Deserialize<GetAniUser>(responseContent);

            // Create a list of AnimeInfo objects
            List<AnimeInfo> animeInfoList = new List<AnimeInfo>();
            
            if (aniUser?.Data?.Page?.MediaList != null)
            {
                foreach (var mediaItem in aniUser.Data.Page.MediaList)
                {
                    var animeInfo = new AnimeInfo
                    {
                        Id = mediaItem.Id,
                        RomajiTitle = mediaItem.Media?.Title?.Romaji,
                        EnglishTitle = mediaItem.Media?.Title?.English,
                        NativeTitle = mediaItem.Media?.Title?.Native
                    };

                    animeInfoList.Add(animeInfo);
                }
            }

            return animeInfoList; // Return the list of AnimeInfo objects
        }
    }
}

