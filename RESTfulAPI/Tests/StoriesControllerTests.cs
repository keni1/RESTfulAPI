using Moq;
using NUnit.Framework;
using RESTfulAPI.Controllers;
using RESTfulAPI.Models;
using RESTfulAPI.Services;

namespace RESTfulAPI.Tests
{
    public class StoriesControllerTests
    {
        private StoriesController? _systemUnderTest;

        private Mock<IStoriesService>? _storiesServiceMock;

        [SetUp]
        public void Setup()
        {
            _storiesServiceMock = new Mock<IStoriesService>(MockBehavior.Strict);
            _systemUnderTest = new StoriesController(_storiesServiceMock.Object);
        }

        // Test naming: Subject - Scenario - Expected Result
        // Test structure: AAA

        // The two tests below could be parameterised
        [Test]
        public async Task GetBestStories_WhenNoBestStoriesIdsAvailable_ShouldReturnEmpty()
        {
            IEnumerable<Story?>? bestStories = new List<Story?>();
            _storiesServiceMock!.Setup(s => s.GetBestStories()).Returns(Task.FromResult(bestStories));

            var actualStories = await _systemUnderTest!.GetBestStories().ConfigureAwait(false);

            Assert.IsNotNull(actualStories);
            Assert.That(actualStories!.Count, Is.EqualTo(0));

            _storiesServiceMock.VerifyAll();
        }

        [Test]
        public async Task GetBestStoriesIds_WhenInvoked_ShouldReturnOneBestStory()
        {
            IEnumerable<Story?>? bestStories = new List<Story?>()
            {
                new Story(),
            };
            _storiesServiceMock!.Setup(s => s.GetBestStories()).Returns(Task.FromResult(bestStories));

            var actualStories = await _systemUnderTest!.GetBestStories().ConfigureAwait(false);

            Assert.IsNotNull(actualStories);
            Assert.That(actualStories!.Count, Is.EqualTo(1));

            _storiesServiceMock.VerifyAll();
        }
    }
}
