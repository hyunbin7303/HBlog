using HBlog.Domain.Entities;
using HBlog.Infrastructure.Authentications;
using HBlog.Infrastructure.Authentications.OAuth;
using HBlog.Infrastructure.Authentications.OAuth.Options;
using HBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace HBlog.Infrastructure.Extensions
{
    public static class HBlogPolicy
    {
        public const string RequireAdminRole = "HBlog.RequireAdminRole";
        public const string AdminModeratorRole = "HBlog.AdminModeratorRole";
    }

    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, string token, IConfiguration config)
        {

            services.AddScoped<ITokenService>(x => new TokenService(x.GetRequiredService<UserManager<User>>(), token));

            services.Configure<GoogleAuthOptions>(config.GetSection(GoogleAuthOptions.SectionName));
            services.Configure<AppleAuthOptions>(config.GetSection(AppleAuthOptions.SectionName));
            services.Configure<OAuthTicketOptions>(opt =>
            {
                config.GetSection(OAuthTicketOptions.SectionName).Bind(opt);
                var envKey = Environment.GetEnvironmentVariable("OAUTH_TICKET_SIGNING_KEY");
                if (!string.IsNullOrEmpty(envKey)) opt.SigningKey = envKey;
            });
            services.AddSingleton<IOAuthTicketService, OAuthTicketService>();
            services.AddSingleton<IExternalIdentityProvider, GoogleIdentityProvider>();
            services.AddSingleton<IExternalIdentityProvider, AppleIdentityProvider>();
            services.AddSingleton<IExternalIdentityProviderRegistry, ExternalIdentityProviderRegistry>();

            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddEntityFrameworkStores<IdentityContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {   
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                }
                
            );

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(HBlogPolicy.RequireAdminRole, policy => policy.RequireRole("Admin"));
                opt.AddPolicy(HBlogPolicy.AdminModeratorRole, policy => policy.RequireRole("Admin", "Moderator"));
            });
            return services;
        }
        
    }
}