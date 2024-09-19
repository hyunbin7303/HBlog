using HBlog.Application.Automapper;
using HBlog.Domain.Entities;
using HBlog.Infrastructure.Data;
using HBlog.Infrastructure.Extensions;
using HBlog.Infrastructure.Middlewares;
using HBlog.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

public class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Current Env: " + Environment.GetEnvironmentVariable("Environment"));
        var builder = WebApplication.CreateBuilder(args);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); // TODO Might need to be removed and find solution for UTC TIme set issue..
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("auth", new OpenApiSecurityScheme
            {
                Name = "auth",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Description = "A one time token. "
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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

        builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AwsSettings"));

        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
        builder.Services.AddIdentityServices(builder.Configuration);
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        var app = builder.Build();
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
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
            if (Environment.GetEnvironmentVariable("Environment") != "test")
            {
                await context.Database.MigrateAsync();
                await Seed.SeedUsers(userManager, roleManager);
                await Seed.SeedCategories(context);
                await Seed.SeedPosts(context);
                await Seed.SeedTags(context);
                await Seed.SeedPostTags(context);
                await context.Database.EnsureCreatedAsync();

            }
        }
        catch (Exception ex)
        {
            var logger = services.GetService<ILogger<Program>>();
            logger.LogError(ex, "An error occured during migration");
        }


        app.Run();
    }
}