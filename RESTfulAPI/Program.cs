using RESTfulAPI.ExceptionHandling;
using RESTfulAPI.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers(options =>
            {
                options.Filters.Add<HttpResponseExceptionFilter>();
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // DI
        builder.Services.AddScoped<IStoriesService, StoriesService>();
        builder.Services.AddScoped<HttpClient>();
        builder.Services.AddScoped<IHttpHandler, HttpHandler>();

        builder.Services.AddMemoryCache();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
