namespace RESTfulAPI.Services
{
    public interface IHttpHandler
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
