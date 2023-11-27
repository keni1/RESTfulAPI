using Microsoft.AspNetCore.Mvc;
using RESTfulAPI.Models;
using RESTfulAPI.Services;

namespace RESTfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly IStoriesService _storyService;

        public StoriesController(IStoriesService storyService)
        {
            _storyService = storyService;
        }

        [HttpGet("beststories", Name = "GetBestStories")]
        public async Task<IEnumerable<Story?>?> GetBestStories()
        {
            return await _storyService.GetBestStories();
        }
    }
}
