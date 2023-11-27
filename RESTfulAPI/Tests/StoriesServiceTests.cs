using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using RESTfulAPI.Controllers;
using RESTfulAPI.Models;
using RESTfulAPI.Services;

namespace RESTfulAPI.Tests
{
    public class StoriesServiceTests
    {
        private StoriesService? _systemUnderTest;

        private Mock<ILogger<StoriesController>>? _loggerMock;
        private Mock<IHttpHandler>? _httpHandlerMock;
        private Mock<IMemoryCache>? _memoryCacheMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<StoriesController>>(MockBehavior.Strict);
            _httpHandlerMock = new Mock<IHttpHandler>(MockBehavior.Strict);
            _memoryCacheMock = new Mock<IMemoryCache>(MockBehavior.Strict);

            _systemUnderTest = new StoriesService(_loggerMock.Object, _httpHandlerMock.Object, _memoryCacheMock.Object);
        }

        [Test]
        public async Task GetBestStories_WhenNoBestStories_ReturnsEmptyCollection()
        {
            var story = (object)new Story();
            var httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            _httpHandlerMock!.Setup(h => h.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(httpResponseMessage));

            var result = await _systemUnderTest!.GetBestStories();

            Assert.That(result, Is.Not.Null);

            _httpHandlerMock.VerifyAll();
        }

        [Test]
        public async Task GetBestStories_WhenGivenTwoBestStoryIds_ReturnsACollectionWithTwoStories()
        {
            // Stories
            var storyOne = (object)new Story();
            var storyTwo = (object)new Story();
            // Http handler
            var httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var bodyContent = new List<int>{1,2};
            HttpContent httpContent = JsonContent.Create(bodyContent);
            httpResponseMessage.Content = httpContent;
            _httpHandlerMock!.Setup(h => h.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(httpResponseMessage));
            _memoryCacheMock!.Setup(c => c.TryGetValue(1, out storyOne)).Returns(true);
            _memoryCacheMock!.Setup(c => c.TryGetValue(2, out storyTwo)).Returns(true);

            var result = await _systemUnderTest!.GetBestStories();

            Assert.That(result, Is.Not.Null);
            Assert.AreSame(storyOne, result.First());
            Assert.AreSame(storyTwo, result.Last());

            _httpHandlerMock.VerifyAll();
            _memoryCacheMock.VerifyAll();
        }
    }
}
