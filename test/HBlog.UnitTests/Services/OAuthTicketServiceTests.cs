using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HBlog.Infrastructure.Authentications.OAuth;
using HBlog.Infrastructure.Authentications.OAuth.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace HBlog.UnitTests.Services
{
    public class OAuthTicketServiceTests
    {
        private const string SigningKey = "unit-test-oauth-ticket-signing-key-must-be-long-enough-for-hmac-sha256-abcdef";

        private static OAuthTicketService NewService(int lifetimeMinutes = 10)
        {
            var opts = Options.Create(new OAuthTicketOptions
            {
                SigningKey = SigningKey,
                LifetimeMinutes = lifetimeMinutes,
            });
            return new OAuthTicketService(opts);
        }

        [Test]
        public void IssueAndValidate_RoundTrip_ReturnsSameIdentity()
        {
            var svc = NewService();
            var identity = new ExternalIdentity("google", "sub-123", "u@example.com", true, "Ada", "Lovelace");

            var ticket = svc.Issue(identity);
            var validated = svc.Validate(ticket);

            Assert.That(validated, Is.Not.Null);
            Assert.That(validated!.Provider, Is.EqualTo("google"));
            Assert.That(validated.Subject, Is.EqualTo("sub-123"));
            Assert.That(validated.Email, Is.EqualTo("u@example.com"));
            Assert.That(validated.EmailVerified, Is.True);
            Assert.That(validated.GivenName, Is.EqualTo("Ada"));
            Assert.That(validated.FamilyName, Is.EqualTo("Lovelace"));
        }

        [Test]
        public void Validate_TamperedTicket_ReturnsNull()
        {
            var svc = NewService();
            var ticket = svc.Issue(new ExternalIdentity("apple", "sub-1", null, false, null, null));
            var tampered = ticket.Substring(0, ticket.Length - 4) + "AAAA";

            Assert.That(svc.Validate(tampered), Is.Null);
        }

        [Test]
        public void Validate_ExpiredTicket_ReturnsNull()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("provider", "google"),
                    new Claim("sub", "sub-9"),
                    new Claim("email_verified", "false"),
                }),
                NotBefore = DateTime.UtcNow.AddHours(-2),
                Expires = DateTime.UtcNow.AddHours(-1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
            };
            var handler = new JwtSecurityTokenHandler { MapInboundClaims = false };
            var expired = handler.WriteToken(handler.CreateToken(descriptor));

            Assert.That(NewService().Validate(expired), Is.Null);
        }

        [Test]
        public void Validate_WrongSigningKey_ReturnsNull()
        {
            var issuer = NewService();
            var ticket = issuer.Issue(new ExternalIdentity("google", "sub-x", null, false, null, null));

            var otherKey = new OAuthTicketService(Options.Create(new OAuthTicketOptions
            {
                SigningKey = "a-completely-different-signing-key-that-should-not-verify-the-original-ticket-xxxx",
                LifetimeMinutes = 10,
            }));

            Assert.That(otherKey.Validate(ticket), Is.Null);
        }
    }
}
