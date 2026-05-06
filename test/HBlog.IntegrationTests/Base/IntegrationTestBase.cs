using HBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HBlog.IntegrationTests.Base
{
    public abstract class IntegrationTestBase : TestBase
    {
        protected readonly IdentityContext _identityContext;
        protected readonly BlogContext _blogContext;

        public IntegrationTestBase()
        {
            var check = _config.GetConnectionString("DefaultConnection");
            
            var identityOptions = new DbContextOptionsBuilder<IdentityContext>()
                .UseNpgsql(check).Options;
            _identityContext = new IdentityContext(identityOptions);
            
            var blogOptions = new DbContextOptionsBuilder<BlogContext>()
                .UseNpgsql(check).Options;
            _blogContext = new BlogContext(blogOptions);
        }
    }
}