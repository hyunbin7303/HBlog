using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using HBlog.Contract.DTOs.OAuth;
using HBlog.Infrastructure.Authentications.OAuth.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace HBlog.Infrastructure.Authentications.OAuth;

public class AppleIdentityProvider : IExternalIdentityProvider
{
    public const string ProviderName = "apple";
    public string Name => ProviderName;

    private const string AppleIssuer = "https://appleid.apple.com";
    private const string AppleJwksUrl = "https://appleid.apple.com/auth/keys";

    private readonly AppleAuthOptions _options;
    private readonly ILogger<AppleIdentityProvider> _logger;
    private readonly ConfigurationManager<OpenIdConnectConfiguration> _configManager;

    public AppleIdentityProvider(IOptions<AppleAuthOptions> options, ILogger<AppleIdentityProvider> logger)
    {
        _options = options.Value;
        _logger = logger;
        _configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            AppleJwksUrl,
            new JwksRetriever(),
            new HttpDocumentRetriever { RequireHttps = true })
        {
            AutomaticRefreshInterval = TimeSpan.FromHours(24),
            RefreshInterval = TimeSpan.FromMinutes(5),
        };
    }

    public async Task<ExternalIdentity?> VerifyAsync(OAuthLoginDto request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.IdToken) || string.IsNullOrWhiteSpace(request.RawNonce))
        {
            _logger.LogWarning("Apple sign-in request missing identityToken or rawNonce.");
            return null;
        }
        if (_options.AllowedAudiences.Count == 0)
        {
            _logger.LogError("AppleAuth:AllowedAudiences is empty; refusing to verify token.");
            return null;
        }

        var result = await ValidateAsync(request.IdToken, ct, refreshKeys: false);
        if (result is null)
        {
            result = await ValidateAsync(request.IdToken, ct, refreshKeys: true);
            if (result is null) return null;
        }

        var (principal, _) = result.Value;

        var nonceClaim = principal.FindFirst("nonce")?.Value;
        if (string.IsNullOrEmpty(nonceClaim) || !NonceMatches(request.RawNonce!, nonceClaim))
        {
            _logger.LogWarning("Apple identity token nonce mismatch.");
            return null;
        }

        var sub = principal.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(sub)) return null;

        var tokenEmail = principal.FindFirst("email")?.Value;
        var emailVerifiedRaw = principal.FindFirst("email_verified")?.Value;
        var emailVerified = emailVerifiedRaw == "true" || emailVerifiedRaw == "\"true\"";

        return new ExternalIdentity(
            Provider: ProviderName,
            Subject: sub,
            Email: tokenEmail ?? request.Email,
            EmailVerified: emailVerified,
            GivenName: request.FirstName,
            FamilyName: request.LastName);
    }

    private async Task<(System.Security.Claims.ClaimsPrincipal Principal, JwtSecurityToken Jwt)?> ValidateAsync(
        string token,
        CancellationToken ct,
        bool refreshKeys)
    {
        try
        {
            if (refreshKeys) _configManager.RequestRefresh();
            var config = await _configManager.GetConfigurationAsync(ct);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = AppleIssuer,
                ValidateAudience = true,
                ValidAudiences = _options.AllowedAudiences,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = config.SigningKeys,
                ClockSkew = TimeSpan.FromSeconds(30),
            };
            var handler = new JwtSecurityTokenHandler { MapInboundClaims = false };
            var principal = handler.ValidateToken(token, parameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwt) return null;
            return (principal, jwt);
        }
        catch (SecurityTokenSignatureKeyNotFoundException)
        {
            if (!refreshKeys) return null;
            _logger.LogWarning("Apple JWKS refresh did not resolve unknown kid.");
            return null;
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogWarning(ex, "Apple identity token validation failed.");
            return null;
        }
    }

    private static bool NonceMatches(string rawNonce, string nonceClaim)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawNonce));
        var hex = Convert.ToHexString(bytes).ToLowerInvariant();
        if (string.Equals(hex, nonceClaim, StringComparison.OrdinalIgnoreCase)) return true;
        var b64Url = Base64UrlEncoder.Encode(bytes);
        if (string.Equals(b64Url, nonceClaim, StringComparison.Ordinal)) return true;
        return string.Equals(rawNonce, nonceClaim, StringComparison.Ordinal);
    }

    private sealed class JwksRetriever : IConfigurationRetriever<OpenIdConnectConfiguration>
    {
        public async Task<OpenIdConnectConfiguration> GetConfigurationAsync(
            string address, IDocumentRetriever retriever, CancellationToken cancel)
        {
            var jwks = await retriever.GetDocumentAsync(address, cancel);
            var config = new OpenIdConnectConfiguration { JsonWebKeySet = new JsonWebKeySet(jwks) };
            foreach (var key in config.JsonWebKeySet.GetSigningKeys())
                config.SigningKeys.Add(key);
            return config;
        }
    }
}
