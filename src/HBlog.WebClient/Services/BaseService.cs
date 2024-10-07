namespace HBlog.WebClient.Services;

public abstract class BaseService(HttpClient httpClient, ILogger logger)
{
    protected readonly HttpClient _httpClient = httpClient;
    protected readonly ILogger _logger = logger;
}
