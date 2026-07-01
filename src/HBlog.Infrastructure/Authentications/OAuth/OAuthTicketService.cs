using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HBlog.Infrastructure.Authentications.OAuth.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HBlog.Infrastructure.Authentications.OAuth;

public class OAuthTicketService : IOAuthTicketService
{
    private const string ProviderClaim = "provider";
    private const string SubjectClaim = "sub";
    private const string EmailClaim = "email";
    private const string EmailVerifiedClaim = "email_verified";
    private const string GivenNameClaim = "given_name";
    private const string FamilyNameClaim = "family_name";

    private readonly OAuthTicketOptions _options;
    private readonly SymmetricSecurityKey _key;

    public OAuthTicketService(IOptions<OAuthTicketOptions> options)
    {
        _options = options.Value;
        if (string.IsNullOrEmpty(_options.SigningKey))
            throw new InvalidOperationException("OAuthTicket:SigningKey is not configured.");
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
    }

    public string Issue(ExternalIdentity identity)
    {
        var claims = new List<Claim>
        {
            new(ProviderClaim, identity.Provider),
            new(SubjectClaim, identity.Subject),
            new(EmailVerifiedClaim, identity.EmailVerified ? "true" : "false"),
        };
        if (!string.IsNullOrEmpty(identity.Email)) claims.Add(new Claim(EmailClaim, identity.Email));
        if (!string.IsNullOrEmpty(identity.GivenName)) claims.Add(new Claim(GivenNameClaim, identity.GivenName));
        if (!string.IsNullOrEmpty(identity.FamilyName)) claims.Add(new Claim(FamilyNameClaim, identity.FamilyName));

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_options.LifetimeMinutes),
            SigningCredentials = creds,
        };
        var handler = new JwtSecurityTokenHandler { MapInboundClaims = false };
        return handler.WriteToken(handler.CreateToken(descriptor));
    }

    public ExternalIdentity? Validate(string ticket)
    {
        if (string.IsNullOrWhiteSpace(ticket)) return null;

        var parameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _key,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
        };
        var handler = new JwtSecurityTokenHandler { MapInboundClaims = false };
        try
        {
            var principal = handler.ValidateToken(ticket, parameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwt) return null;
            if (!jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                return null;

            var provider = principal.FindFirst(ProviderClaim)?.Value;
            var sub = principal.FindFirst(SubjectClaim)?.Value;
            if (string.IsNullOrEmpty(provider) || string.IsNullOrEmpty(sub)) return null;

            var email = principal.FindFirst(EmailClaim)?.Value;
            var emailVerified = principal.FindFirst(EmailVerifiedClaim)?.Value == "true";
            var given = principal.FindFirst(GivenNameClaim)?.Value;
            var family = principal.FindFirst(FamilyNameClaim)?.Value;
            return new ExternalIdentity(provider, sub, email, emailVerified, given, family);
        }
        catch (SecurityTokenException)
        {
            return null;
        }
    }
}
