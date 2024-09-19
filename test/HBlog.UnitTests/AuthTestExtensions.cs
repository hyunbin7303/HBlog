using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HBlog.UnitTests
{
    public class TestAuthHandlerOptions : AuthenticationSchemeOptions
    {
        public int DefaultUserId { get; set; }
    }
    public class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
    {
        public const int UserId = 1;

        public const string AuthenticationScheme = "TestScheme";
        private readonly int _defaultUserId;

        public TestAuthHandler(
            IOptionsMonitor<TestAuthHandlerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _defaultUserId = options.CurrentValue.DefaultUserId;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim> { new (ClaimTypes.Name, "TestUser") };

            if (Context.Request.Headers.TryGetValue(UserId.ToString(), out var userId))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId[0]));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, _defaultUserId.ToString()));
            }
            var identity = new ClaimsIdentity(claims, AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
