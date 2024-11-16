using System.Text.RegularExpressions;
using HBlog.Application.Services;
using HBlog.Domain.Repositories;
using HBlog.Infrastructure.Authentications;
using HBlog.Infrastructure.Data;
using HBlog.Infrastructure.Helpers;
using HBlog.Infrastructure.Repositories;
using HBlog.Infrastructure.Services;
using HBlog.Infrastructure.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HBlog.Infrastructure.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config){
            services.AddDbContext<DataContext>(opt =>
            {
                if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"){
                    Console.WriteLine(Environment.GetEnvironmentVariable("DATABASE_URL"));
                    var m = Regex.Match(Environment.GetEnvironmentVariable("DATABASE_URL")!, @"postgres://(.*):(.*)@(.*):(.*)/(.*)");
                    opt.UseNpgsql($"Server={m.Groups[3]};Port={m.Groups[4]};User Id={m.Groups[1]};Password={m.Groups[2]};Database={m.Groups[5]};sslmode=Prefer;Trust Server Certificate=true");
                }else {
                    Console.WriteLine("Connection String:" + config.GetConnectionString("DefaultConnection"));
                    opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
                }
            });
            services.AddCors();

            // Repository Layer DI
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IFileDataRepository, FileDataRepository>();
            services.AddScoped<IFileStorageRepository, FileStorageRepository>();

            // Application Service Layer DI
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IAwsStorageService, AwsStorageService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<ITagService, TagService>();


            services.AddScoped<LogUserActivity>();
            services.AddSignalR();

            services.AddSingleton<PresenceTracker>();

            return services;
        }

    }
}