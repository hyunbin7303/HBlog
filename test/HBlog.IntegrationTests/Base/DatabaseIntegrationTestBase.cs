using HBlog.Domain.Entities;
using HBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Testcontainers.PostgreSql;

namespace HBlog.IntegrationTests.Base
{
    public abstract class DatabaseIntegrationTestBase
    {
        protected PostgreSqlContainer _postgresContainer;
        protected IdentityContext _identityContext;
        protected BlogContext _blogContext;
        protected UserManager<User> _userManager;
        protected RoleManager<AppRole> _roleManager;
        protected ServiceProvider _serviceProvider;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            // Create and start PostgreSQL container
            _postgresContainer = new PostgreSqlBuilder("postgres:15")
                .WithDatabase("hblog_test")
                .WithUsername("test_user")
                .WithPassword("test_password")
                .WithCleanUp(true)
                .Build();

            await _postgresContainer.StartAsync();
        }

        [SetUp]
        public async Task SetUp()
        {
            var services = new ServiceCollection();
            services.AddDbContext<IdentityContext>(options =>
                options.UseNpgsql(_postgresContainer.GetConnectionString()));
            
            services.AddDbContext<BlogContext>(options =>
                options.UseNpgsql(_postgresContainer.GetConnectionString()));

            services.AddIdentity<User, AppRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.AddLogging(builder => builder.AddConsole());

            _serviceProvider = services.BuildServiceProvider();
            _identityContext = _serviceProvider.GetRequiredService<IdentityContext>();
            _blogContext = _serviceProvider.GetRequiredService<BlogContext>();
            _userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
            _roleManager = _serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            // Ensure databases are created and migrations are applied
            await _identityContext.Database.EnsureCreatedAsync();
            await _blogContext.Database.EnsureCreatedAsync();
            
            // Seed basic test data
            await SeedTestData();
        }

        [TearDown]
        public async Task TearDown()
        {
            // Clean up after each test
            await _identityContext.Database.EnsureDeletedAsync();
            await _blogContext.Database.EnsureDeletedAsync();
            await _identityContext.DisposeAsync();
            await _blogContext.DisposeAsync();
            _serviceProvider?.DisposeAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await _postgresContainer.DisposeAsync();
        }

        protected virtual async Task SeedTestData()
        {
            // Create test roles
            var adminRole = new AppRole { Name = "Admin" };
            var memberRole = new AppRole { Name = "Member" };
            
            await _roleManager.CreateAsync(adminRole);
            await _roleManager.CreateAsync(memberRole);

            // Create test users
            var testUser1 = new User
            {
                UserName = "testuser1",
                Email = "test1@example.com",
                FirstName = "Test",
                LastName = "User1",
                Created = DateTime.UtcNow,
                LastActive = DateTime.UtcNow
            };

            var testUser2 = new User
            {
                UserName = "testuser2",
                Email = "test2@example.com",
                FirstName = "Test",
                LastName = "User2",
                Created = DateTime.UtcNow,
                LastActive = DateTime.UtcNow
            };

            await _userManager.CreateAsync(testUser1, "TestPassword123!");
            await _userManager.CreateAsync(testUser2, "TestPassword123!");
            
            // Add users to roles
            await _userManager.AddToRoleAsync(testUser1, "Member");
            await _userManager.AddToRoleAsync(testUser2, "Member");

            // Create test categories
            var categories = new List<Category>
            {
                new() { Title = "Technology" },
                new() { Title = "Programming" }
            };
            
            _blogContext.Categories.AddRange(categories);

            // Create test tags
            var tags = new List<Tag>
            {
                new() { Name = "C#" },
                new() { Name = "Blazor" },
                new() { Name = "ASP.NET" }
            };
            
            _blogContext.Tags.AddRange(tags);
            await _blogContext.SaveChangesAsync();
        }

        protected async Task<User> GetTestUserAsync(string username = "testuser1")
        {
            return await _userManager.FindByNameAsync(username);
        }

        protected async Task<Category> GetTestCategoryAsync(string title = "Technology")
        {
            return await _blogContext.Categories.FirstOrDefaultAsync(c => c.Title == title);
        }

        protected async Task<Tag> GetTestTagAsync(string name = "C#")
        {
            return await _blogContext.Tags.FirstOrDefaultAsync(t => t.Name == name);
        }
    }
}