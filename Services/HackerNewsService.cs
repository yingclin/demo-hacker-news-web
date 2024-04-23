using HackerNewsWeb.Models;
using Newtonsoft.Json;

namespace HackerNewsWeb.Services
{
    public class HackerNewsService
    {
        private const string TopStoriesUrl = "https://hacker-news.firebaseio.com/v0/topstories.json";
        private const string NewStoriesUrl = "https://hacker-news.firebaseio.com/v0/newstories.json";
        private const string BestStoriesUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";
        private const string ItemBaseUrl = "https://hacker-news.firebaseio.com/v0/item/";
        private const int PageSize = 30;
        //
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<(long[], int)> GetTopStories(int page = 1)
        {
            if(page <= 0)
            {
                return (Array.Empty<long>(), 0);
            }

            var response = await _httpClient.GetAsync(TopStoriesUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    var allItems = JsonConvert.DeserializeObject<long[]>(jsonResponse);
                    var pageCount = (int)Math.Ceiling(((double)allItems.Length / PageSize));
                    var skipCount = (page - 1) * PageSize;

                    if (page <= pageCount)
                    {
                        return (allItems.Skip(skipCount).Take(PageSize).ToArray(), pageCount);
                    }
                }
            }

            return (Array.Empty<long>(), 0);
        }

        public async Task<List<NewsItem>> GetItems(long[] ids)
        {
            var items = new List<NewsItem>();

            foreach (long id in ids )
            { 
                var item = await GetItemOrNull(id);

                if(item != null)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        public async Task<NewsItem?> GetItemOrNull(long id)
        {
            if(id <= 0)
            {
                return null;
            }

            var response = await _httpClient.GetAsync($"{ItemBaseUrl}{id}.json");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    return JsonConvert.DeserializeObject<NewsItem>(jsonResponse);
                }
            }

            return null;
        }
    }
}
