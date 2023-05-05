using KevBlog.Application.Services;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Authentications;
using KevBlog.Infrastructure.Data;
using KevBlog.Infrastructure.Helpers;
using KevBlog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KevBlog.Infrastructure.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config){
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            services.AddCors();

            // Repository Layer DI
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<ITagRepository, TagRepository>();

            // Application Service Layer DI
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IMessageService, MessageService>();

            services.AddScoped<LogUserActivity>();
            services.AddSignalR();


            return services;
        }
        
    }
}