namespace HBlog.Infrastructure.Authentications.OAuth.Options;

public class AppleAuthOptions
{
    public const string SectionName = "AppleAuth";
    public List<string> AllowedAudiences { get; set; } = new();
}
