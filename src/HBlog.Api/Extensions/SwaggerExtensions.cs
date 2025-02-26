using Microsoft.OpenApi.Models;
using System.Reflection;

namespace HBlog.Api.Extensions
{
    internal static class SwaggerExtensions
    {
        internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HBlog API",
                    Version = "v1",
                    Description = "A simple blog API",
                });

                options.AddSecurityDefinition("auth", new OpenApiSecurityScheme
                {
                    Name = "auth",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Description = "JWT Authorization header using the Bearer scheme. Example: \\\"Authorization: Bearer {token}\\\""
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "auth",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });
            return services;
        }

    }
}
