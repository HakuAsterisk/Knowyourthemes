using System.Text;
using System.Text.Json;
using GuessTheBeat.Components.Models;
using System.Text.RegularExpressions;
namespace GuessTheBeat.Components.Services
{
    public class AnimeInfo //Local class for the AniList / MAL API responses
    {
        public int Id { get; set; }
    }
    public class AnimeResult //Local class for the AnimeThemes API response
    {
        public string? AnimeName { get; set; }
        public string? VideoLink { get; set; }
        public string? AudioLink { get; set; }
    }

    public class FoundResult //Local class for the AnimeThemes API search endpoint response
    {
        public string? ResultName { get; set; }
    }

    public class AnimeSongService
    {
        private readonly HttpClient _httpClient; //client for API calls
        public AnimeSongService(HttpClient httpClient)
        {
        _httpClient = httpClient;
        }
        private static readonly Random _random = new Random(); //Random for randomizing the order of ID's
        private int currentPage = 1;
        private int perPage = 50;   //pagination variables
        private bool hasNextPage = true;
        private List<AnimeInfo> animeInfoList = new List<AnimeInfo>();  //List for carrying AnimeInfo objects
        
        //------------------MYANIMELIST------------------

        public async Task<List<AnimeInfo>> GetMediaListAsync(string username) //Gets a list of Anime ID's from the MAL API IN s.username
        {
            animeInfoList.Clear(); //Clear animeInfoList as a measure to prevent accidental crashes from user.
            //Build the URL
            var malUrl = "https://api.myanimelist.net/v2/users/" + username + "/animelist?limit=50&status=completed&fields=media_type";

            var request = new HttpRequestMessage(HttpMethod.Get, malUrl);
            request.Headers.Add("X-MAL-CLIENT-ID", "ClientID"); //Add headers to verify API call

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode(); //Wait for the response

            var responseContent = await response.Content.ReadAsStringAsync();
            var root = JsonSerializer.Deserialize<GetMalUser>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            //Deserialize the Json response with the GetMalUser method
            if (root.Data != null)
            {
                foreach (var mediaItem in root.Data)
                {
                    if(mediaItem.Node?.MediaType == "tv"){
                    var animeInfo = new AnimeInfo
                    {
                        Id = mediaItem.Node.Id, 
                        //If data was recieved, take any data with "tv" in MediaType header and add it's ID value to a new AnimeInfo object
                    };
                    //Make a list of AnimeInfo objects
                    animeInfoList.Add(animeInfo);
                    }
                }
            }

            //Shuffle and return 10 unique titles from the AnimeInfoList
            var randomizedList = animeInfoList
                .OrderBy(x => _random.Next()) //Shuffle
                .DistinctBy(x => x.Id)        //Ensure uniqueness
                .Take(10)                     //Take first 10
                .ToList();

            return randomizedList; //Return list of 10 unique ID's in random order. OUT L.AnimeInfo
        }

        //--------------------ANILIST POPULAR---------------------
        public async Task<List<AnimeInfo>> GetPopularAsync() //Grab a list of 150 popular anime from the AniList API
        {
            animeInfoList.Clear(); //Clear animeInfoList as a measure to prevent accidental crashes from user.
            while (currentPage <= 3) //We are taking 3 pages of 50 shows each from the API to ensure a rich pool of anime to shuffle from
            {
                var query = $@"
                query{{
                    Page(page: {currentPage}, perPage: {perPage}) {{
                        media(sort: [POPULARITY_DESC, FORMAT_DESC], format_in: [TV]) {{
                        id
                        format
                        }}
                    }}
                }}";

                var requestBody = new //Build a new query based on the one created above that asks for a list of media sorted by
                {                     //popularity descending, then format descending (tv first)
                    query = query,
                    variables = new { }
                };

                //Convert the request body to JSON
                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //Set the request URL
                var url = "https://graphql.anilist.co";

                //Send the Json request
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode(); // Throw if not a success code.

                //Read the response content
                var responseContent = await response.Content.ReadAsStringAsync();

                //Deserialize the response with the GetAniUser model
                var aniUser = JsonSerializer.Deserialize<GetAniUser>(responseContent);
                
                if (aniUser?.Data?.Page?.Media != null)
                {
                    foreach (var mediaItem in aniUser.Data.Page.Media)
                    {
                        if(mediaItem.Format == "TV"){
                        var animeInfo = new AnimeInfo
                        {
                            Id = mediaItem.Id,
                            //If data was recieved, take any data with "tv" in MediaType header and add it's ID value to a new AnimeInfo object
                        };

                        animeInfoList.Add(animeInfo);
                        }
                    }
                }
                currentPage++; //Move to the next page
            }
            var randomizedList = animeInfoList
                                    .OrderBy(x => _random.Next())  //Shuffle
                                    .DistinctBy(x => x.Id)        //Ensure uniqueness
                                    .Take(10)                    //Take first 10
                                    .ToList();
            return randomizedList;  //Return list of 10 unique ID's in random order. OUT L.AnimeInfo
        }

        //--------------------ANILIST---------------------
        public async Task<List<AnimeInfo>> GetAniListAsync(string username) //Grab a users animelist from the AniList API IN s.username
        {
            animeInfoList.Clear();
            while (hasNextPage) //Keep pulling anime for as long as there are pages to go through.
            {
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
                            }}
                            score
                            status
                        }}
                    }}
                }}";

                var requestBody = new //Build a new query based on the one created above that asks for a list of media sorted by
                {                     //A users list of completed anime
                    query = query
                };

                //Convert the request body to JSON
                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //Set the request URL
                var url = "https://graphql.anilist.co";

                //Send the POST request
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode(); // Throw if not a success code.

                //Read the response content
                var responseContent = await response.Content.ReadAsStringAsync();

                //Deserialize the response into the GetAniUser model
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
                                Id = mediaItem.Media.Id, //Add AnimeInfo object into animeInfoList
                            };

                            animeInfoList.Add(animeInfo);
                        }
                    }

                    //Check if there's another page to fetch
                    hasNextPage = aniUser.Data.Page.PageInfo.HasNextPage;
                }

                else
                {
                    hasNextPage = false; //Stop if no data
                }

                currentPage++; //Move to the next page
            }

            var randomizedList = animeInfoList
                                .OrderBy(x => _random.Next())   //Shuffle
                                .DistinctBy(x => x.Id)         //Ensure uniqueness
                                .Take(10)                     //Take first 10
                                .ToList();
            return randomizedList;  //Return list of 10 unique ID's in random order. OUT L.AnimeInfo
        }

        //---------------------------- FETCH ANIMETHEMES INFO---------------------------
        public async Task<List<AnimeResult>> ConvertId(int oldId, string site) //Convert MAL and AniList ID's into AnimeThemes content
        {                                                                      //IN i.oldId, s.site
            //Base URL for the AnimeThemes API
            string apiUrl = "https://api.animethemes.moe/anime?filter[has]=resources&filter[site]=";

            //Select the correct filter (MyAnimeList or AniList)
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

            //Call the API and ensure success
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            //Deserialize the JSON response into the root object
            var root = JsonSerializer.Deserialize<GetThemeResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var animeResults = new List<AnimeResult>();

            //Regex to match OP# and ED#
            //This way we do not end up with a list populated by insert songs or bonus records from physical disks etc.
            //This also allows us to eventually add the inclusion of other options than the first opening to a show
            var opEdRegex = new Regex(@"^(OP|ED)\d+$", RegexOptions.IgnoreCase);

            //Set to keep track of slugs we have already processed
            var slugSeen = new HashSet<string>();

            //Ensure we have anime data
            if (root?.Anime != null && root.Anime.Any())
            {
                var anime = root.Anime.FirstOrDefault();

                if (anime != null)
                {
                    //Attempt to find the English synonym, if it exists. This lets us use names our users are more used to
                    //instead of having to know the romanji title (ex. Sousou no Frieren) they can use the english one
                    //ex. Frieren: Beyond Journey's End
                    var englishSynonym = anime.Synonyms?
                        .FirstOrDefault(s => s.Type != null && s.Type.Equals("English", StringComparison.OrdinalIgnoreCase));

                    //Fallback to the original anime name if no English synonym exists
                    string animeName = englishSynonym?.Text ?? anime.Name;

                    if (anime.AnimeThemes != null)
                    {
                        foreach (var theme in anime.AnimeThemes)
                        {
                            //Check if the theme slug matches "OP#" or "ED#" and has not been seen before
                            if (theme.Slug != null && opEdRegex.IsMatch(theme.Slug) && !slugSeen.Contains(theme.Slug))
                            {
                                //Add the theme slug to the seen set to avoid duplicates
                                //This way OP1 and OP1_tv dont end up on the same list
                                slugSeen.Add(theme.Slug);

                                if (theme.AnimeThemeEntries != null)
                                {
                                    foreach (var entry in theme.AnimeThemeEntries)
                                    {
                                        if (entry.Videos != null)
                                        {
                                            foreach (var video in entry.Videos)
                                            {
                                                //Create the AnimeResult object with the English name or fallback name
                                                var animeResult = new AnimeResult
                                                {
                                                    AnimeName = animeName,        //Use the English synonym if available
                                                    VideoLink = video.Link,       //Otherwise add data to the AnimeResult object
                                                    AudioLink = video.Audio?.Link
                                                };

                                                //Add this result to the list
                                                animeResults.Add(animeResult);

                                                //Exit the loop once the first version of OP/ED has been added
                                                break;
                                            }

                                            //Exit once we've handled the first entry for this theme
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return animeResults; //Return the object with the name and base OPs and EDs added to the list. OUT L.AnimeResult
        }

        //------------- AUTO-COMPLETE SEARCH ----------------
        public async Task<List<FoundResult>> SearchAnimeAsync(string query) //Use the AnimeThemes Search endpoint to look for
        {                                                                   //names similar to the search query and return them as a list IN s.query
            //Build the API query
            var response = await _httpClient.GetFromJsonAsync<SearchResult>($"https://api.animethemes.moe/search?fields[search]=anime&q={query}&include[anime]=animesynonyms");

            //Create new list to contain the results
            var foundResults = new List<FoundResult>();

            //Check that Info and Data are not null
            if (response?.Info?.Data != null)
            {
                //Loop through each anime entry
                foreach (var data in response.Info.Data)
                {
                    //Attempt to find the English synonym
                    var englishSynonym = data.Synonyms?.FirstOrDefault(s => s.Type != null && s.Type.Equals("English", StringComparison.OrdinalIgnoreCase));

                    //Fallback to the original anime name if no English synonym exists
                    string animeName = englishSynonym?.Text ?? data.Name;

                    var foundResult = new FoundResult
                    {
                        ResultName = animeName
                    };

                    foundResults.Add(foundResult); //Add name to FoundResult object and the object into the list
                }
            }
            //Return the list of found results OUT L.FoundResult
            return foundResults;
        }
    }
}

