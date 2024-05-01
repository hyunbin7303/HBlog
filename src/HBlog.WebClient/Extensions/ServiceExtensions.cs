using HBlog.WebClient.Services;

namespace HBlog.WebClient.Extensions;
public static class ServiceExtensions
{
    public static void RegisterClientServices(this IServiceCollection services)
    {
        services.AddSingleton<MarkdownService>();
        services.AddScoped<IPostService, PostClientService>();
        services.AddScoped<ICategoryService, CategoryClientService>();
        services.AddScoped<ITagService, TagClientService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<UserClientService>();
    }
}
