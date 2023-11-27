using Microsoft.Extensions.Caching.Memory;
using RESTfulAPI.Controllers;
using RESTfulAPI.Models;
using System.Text.Json;

namespace RESTfulAPI.Services
{
    public class StoriesService : IStoriesService
    {
        private const int AbsoluteExpirationRelativeToNowInSeconds = 180;

        private readonly ILogger<StoriesController> _logger;
        private readonly IHttpHandler _httpHandler;
        private readonly IMemoryCache _memoryCache; // We could abstract out caching to a new class instead of directly using IMemoryCache.
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public StoriesService(ILogger<StoriesController> logger, IHttpHandler httpHandler, IMemoryCache memoryCache)
        {
            _logger = logger;
            _httpHandler = httpHandler;
            _memoryCache = memoryCache;
            _cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = new TimeSpan(0, 0, AbsoluteExpirationRelativeToNowInSeconds)
            };        
        }

        public async Task<List<int>?> GetBestStoriesIds()
        {
            var url = @"https://hacker-news.firebaseio.com/v0/beststories.json";

            var response = await _httpHandler.GetAsync(url).ConfigureAwait(false);
            
            List<int>? bestStoriesIds = null; // An Empty collection usually means nothing to return and no errors, but a null return could mean errors happened.

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    var storiesIds = JsonSerializer.Deserialize<List<int>>(jsonString);
                    if (storiesIds != null)
                        bestStoriesIds = storiesIds;
                    else
                        _logger.LogError(@$"JSON string deserialised to null or empty Id list");
                }
            }
            else
            {
                _logger.LogError(@$"{(int)response.StatusCode} {response.ReasonPhrase}");
            }

            return bestStoriesIds;
        }

        public async Task<Story?> GetStory(int storyId)
        {
            var url = @$"https://hacker-news.firebaseio.com/v0/item/{storyId}.json";

            var response = await _httpHandler.GetAsync(url).ConfigureAwait(false);
            
            Story? story = null;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (jsonString != null) 
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    story = JsonSerializer.Deserialize<Story>(jsonString, options);
                }
            }

            return story;
        }

        public async Task<IEnumerable<Story?>> GetBestStories()
        {
            var stories = new List<Story?>();

            var storiesIds = await GetBestStoriesIds();
            if (storiesIds == null)
                return stories;

            foreach (var storyId in storiesIds)
            {
                if (!_memoryCache.TryGetValue(storyId, out Story? story))
                {
                    story = await GetStory(storyId);
                    _memoryCache.Set(storyId, story, _cacheEntryOptions);
                }

                stories.Add(story);
            }

            return stories;
        }
    }
}
