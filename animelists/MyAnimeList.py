import requests
import time

CLIENT_ID = 'your_client_id'  # Replace with your MAL Client ID
limit = 100  # Maximum number of anime entries per request
offset = 0   # Start at the first entry
anime_list = []

while True:
    url = f'https://api.myanimelist.net/v2/anime?limit=100&offset={offset}&fields=title'
    CLIENT_ID = 'f1310d1d225d6ec4f0107e341a2788ad'
    headers = {'X-MAL-Client-ID': CLIENT_ID}
    response = requests.get(url, headers=headers)
    
    if response.status_code != 200:
        print(f"Error: {response.status_code}")
        break

    data = response.json()
    anime_list.extend([anime['title'] for anime in data['data']])

    # Break the loop if there are no more anime in the response
    if len(data['data']) < limit:
        break

    offset += limit  # Increment the offset for pagination
    time.sleep(1)  # Be kind to the server, avoid hitting rate limits

# Save to a .txt file
with open('mal_anime_list.txt', 'w', encoding='utf-8') as f:
    for anime in anime_list:
        f.write(f"{anime}\n")

print(f"Saved {len(anime_list)} anime titles to mal_anime_list.txt")
