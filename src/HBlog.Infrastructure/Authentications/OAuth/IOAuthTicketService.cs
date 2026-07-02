namespace HBlog.Infrastructure.Authentications.OAuth;

public interface IOAuthTicketService
{
    string Issue(ExternalIdentity identity);
    ExternalIdentity? Validate(string ticket);
}
