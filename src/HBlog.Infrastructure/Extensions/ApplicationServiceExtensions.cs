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
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, string connStr){
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseNpgsql(connStr);
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