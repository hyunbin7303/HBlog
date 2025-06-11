#nullable enable
using System.Diagnostics;
using HBlog.Api.Extensions;
using HBlog.Application.Automapper;
using HBlog.Domain.Entities;
using HBlog.Infrastructure.Data;
using HBlog.Infrastructure.Extensions;
using HBlog.Infrastructure.Middlewares;
using HBlog.Infrastructure.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Console.WriteLine("ASPNETCORE EMV: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVRIONMENT") + " or ENV NAME:" + builder.Environment.EnvironmentName);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); // TODO Might need to be removed and find solution for UTC TIme set issue..
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

        builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AwsSettings"));
        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddAutoMapper(o => o.AddProfile(typeof(AutoMapperProfiles)));
        builder.Services.AddSwaggerDocumentation();
        builder.Services.AddIdentityServices(builder.Configuration);
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        var app = builder.Build();
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
        if(!app.Environment.IsDevelopment())
            app.UseExceptionHandler("/Error");

        //app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
        app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:7183", "http://localhost:5050"));
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<DataContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
            await context.Database.MigrateAsync();

            await Seed.SeedRoles(roleManager);
            if (builder.Environment.IsProduction() is false)
            {
                await Seed.SeedUsers(userManager);
                await Seed.SeedCategories(context);
                await Seed.SeedPosts(context);
                await Seed.SeedTags(context);
                await Seed.SeedPostTags(context);
            }
            await context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            var logger = services.GetService<ILogger<Program>>();
            logger.LogError(ex, "An error occured during migration");
        }


        app.Run();
    }
}