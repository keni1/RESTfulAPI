namespace RESTfulAPI.Services
{
    public class HttpHandler : IHttpHandler
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }
    }
}
