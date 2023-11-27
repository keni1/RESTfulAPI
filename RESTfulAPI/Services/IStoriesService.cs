using RESTfulAPI.Models;

namespace RESTfulAPI.Services
{
    public interface IStoriesService
    {
        public Task<List<int>?> GetBestStoriesIds();

        Task<Story?> GetStory(int storyId);

        Task<IEnumerable<Story?>> GetBestStories();
    }
}
