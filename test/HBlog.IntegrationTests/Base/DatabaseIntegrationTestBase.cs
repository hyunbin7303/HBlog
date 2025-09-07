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
        protected DataContext _dbContext;
        protected UserManager<User> _userManager;
        protected RoleManager<AppRole> _roleManager;
        protected ServiceProvider _serviceProvider;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            // Create and start PostgreSQL container
            _postgresContainer = new PostgreSqlBuilder()
                .WithImage("postgres:15")
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
            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(_postgresContainer.GetConnectionString()));

            services.AddIdentity<User, AppRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.AddLogging(builder => builder.AddConsole());

            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetRequiredService<DataContext>();
            _userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
            _roleManager = _serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            // Ensure database is created and migrations are applied
            await _dbContext.Database.EnsureCreatedAsync();
            
            // Seed basic test data
            await SeedTestData();
        }

        [TearDown]
        public async Task TearDown()
        {
            // Clean up after each test
            await _dbContext.Database.EnsureDeletedAsync();
            await _dbContext.DisposeAsync();
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
            
            _dbContext.Categories.AddRange(categories);

            // Create test tags
            var tags = new List<Tag>
            {
                new() { Name = "C#" },
                new() { Name = "Blazor" },
                new() { Name = "ASP.NET" }
            };
            
            _dbContext.Tags.AddRange(tags);
            await _dbContext.SaveChangesAsync();
        }

        protected async Task<User> GetTestUserAsync(string username = "testuser1")
        {
            return await _userManager.FindByNameAsync(username);
        }

        protected async Task<Category> GetTestCategoryAsync(string title = "Technology")
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(c => c.Title == title);
        }

        protected async Task<Tag> GetTestTagAsync(string name = "C#")
        {
            return await _dbContext.Tags.FirstOrDefaultAsync(t => t.Name == name);
        }
    }
}