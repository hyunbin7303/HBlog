using HBlog.WebClient.Services;

namespace HBlog.WebClient.Providers;
public class HttpServiceProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILoggerFactory _loggerFactory;

    public HttpServiceProvider(HttpClient httpClient, ILoggerFactory loggerFactory)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
    }

    public T GetService<T>() where T : BaseService
    {
        var logger = _loggerFactory.CreateLogger<T>();
        return (T)Activator.CreateInstance(typeof(T), _httpClient, logger)!;
    }

    public List<BaseService> GetAllServices()
    {
        // Return a list of all service instances
        var services = new List<BaseService>
        {
            GetService<PostClientService>(),
            GetService<CategoryClientService>(),
            GetService<TagClientService>(),
            // Add other services here

        };

        return services;
    }
}

