// Components/Services/AnimeSongService.cs
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GuessTheBeat.Components.Models;

namespace GuessTheBeat.Components.Services
{
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

        public async Task<List<MediaList>> GetMediaListAsync(string apiUrl, string query)
        {
            var requestContent = new StringContent(query, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiUrl, requestContent);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var root = JsonSerializer.Deserialize<GetUser>(responseContent);

            return root.Page.MediaList;
        }
    }
}
