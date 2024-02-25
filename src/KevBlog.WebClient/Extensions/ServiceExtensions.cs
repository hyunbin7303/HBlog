using KevBlog.WebClient.Services;

namespace KevBlog.WebClient.Extensions;
public static class ServiceExtensions
{
    public static void RegisterClientServices(this IServiceCollection services)
    {
        services.AddScoped<MarkdownService>();
        services.AddScoped<IHttpService, BaseHttpService>();
        services.AddScoped<IPostService, PostClientService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<UserClientService>();
    }
}
