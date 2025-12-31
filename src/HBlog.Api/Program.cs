#nullable enable
using HBlog.Api.Extensions;
using HBlog.Application.Automapper;
using HBlog.Infrastructure.Data;
using HBlog.Infrastructure.Extensions;
using HBlog.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Diagnostics;
using System.Text.RegularExpressions;

public class Program
{
    private static string? _token = string.Empty;
    private static string? _connStr = string.Empty;
    private static IConfiguration _config;
    private static void SetCredentials(string env)
    {
        if (env == "dev" || env == "Development" || string.IsNullOrEmpty(env))
        {
            _token = _config["TokenKey"];
            _connStr = _config.GetConnectionString("DefaultConnection");
        }
        else
        {
            _token = Environment.GetEnvironmentVariable("TOKEN_KEY");
            var m = Regex.Match(Environment.GetEnvironmentVariable("DATABASE_URL")!, @"postgres://(.*):(.*)@(.*):(.*)/(.*)");
            _connStr =
                $"Server={m.Groups[3]};Port={m.Groups[4]};User Id={m.Groups[1]};Password={m.Groups[2]};Database={m.Groups[5]};sslmode=Prefer;Trust Server Certificate=true";
        }
        Console.WriteLine("Token:" + _token);
        Console.WriteLine("ConnectionString:" + _connStr);
    }

    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        _config = builder.Configuration;
        Console.WriteLine("Env:" + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        SetCredentials(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!);
        var isProd = builder.Environment.IsProduction();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddProblemDetails(options =>
        {

            options.CustomizeProblemDetails = (context) =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<IdentityContext>((serviceProvider, opt) =>
        {
            opt.UseNpgsql(_connStr, config =>
            {
                config.CommandTimeout(30);
                config.MigrationsHistoryTable("__EFMigrationHistory_Identity");
                config.MigrationsAssembly(typeof(IdentityContext).Assembly);
            });
        });

        builder.Services.AddDbContext<BlogContext>((serviceProvider, opt) =>
        {
            opt.UseNpgsql(_connStr, config =>
            {
                config.CommandTimeout(30);
                config.MigrationsHistoryTable("__EFMigrationHistory_Blog");
                config.MigrationsAssembly(typeof(BlogContext).Assembly);
            });
        });
        
        builder.Services.AddCors();
        builder.Services.AddApplicationServices();
        builder.Services.AddIdentityServices(_token);
        builder.Services.AddAutoMapper(o => o.AddProfile(typeof(AutoMapperProfiles)));
        builder.Services.AddSwaggerDocumentation();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        var app = builder.Build();
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
        if (isProd)
            app.UseExceptionHandler("/Error");

        // Add Scalar for OpenApi
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.EnabledClients = [ScalarClient.Curl, ScalarClient.HttpClient, ScalarClient.Axios, ScalarClient.Fetch, ScalarClient.Wget, ScalarClient.WebRequest, ScalarClient.RestMethod];
            options.EnabledTargets = [ScalarTarget.Node, ScalarTarget.CSharp, ScalarTarget.Shell, ScalarTarget.PowerShell];
        });

        //app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
        app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:7183", "http://localhost:5050"));
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        await Seed.SeedData(services, isProd);
        await app.RunAsync();
    }
}
