using HBlog.Application.Services;
using HBlog.Domain.Repositories;
using HBlog.Infrastructure.Helpers;
using HBlog.Infrastructure.Repositories;
using HBlog.Infrastructure.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace HBlog.Infrastructure.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services){

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<ITagService, TagService>();


            services.AddScoped<LogUserActivity>();
            services.AddSignalR();

            services.AddSingleton<PresenceTracker>();

            return services;
        }

    }
}