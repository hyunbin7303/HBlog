namespace HBlog.Infrastructure.Authentications.OAuth.Options;

public class GoogleAuthOptions
{
    public const string SectionName = "GoogleAuth";
    public List<string> AllowedAudiences { get; set; } = new();
}
