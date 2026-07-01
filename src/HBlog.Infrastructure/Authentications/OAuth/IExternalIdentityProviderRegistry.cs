namespace HBlog.Infrastructure.Authentications.OAuth;

public interface IExternalIdentityProviderRegistry
{
    IExternalIdentityProvider? Find(string name);
}

public class ExternalIdentityProviderRegistry : IExternalIdentityProviderRegistry
{
    private readonly Dictionary<string, IExternalIdentityProvider> _byName;

    public ExternalIdentityProviderRegistry(IEnumerable<IExternalIdentityProvider> providers)
    {
        _byName = providers.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
    }

    public IExternalIdentityProvider? Find(string name) =>
        !string.IsNullOrEmpty(name) && _byName.TryGetValue(name, out var p) ? p : null;
}
