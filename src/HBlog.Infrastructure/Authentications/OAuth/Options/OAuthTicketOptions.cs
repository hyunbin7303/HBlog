namespace HBlog.Infrastructure.Authentications.OAuth.Options;

public class OAuthTicketOptions
{
    public const string SectionName = "OAuthTicket";
    public string SigningKey { get; set; } = string.Empty;
    public int LifetimeMinutes { get; set; } = 10;
}
