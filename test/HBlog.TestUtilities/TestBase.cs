using System.Resources;
using AutoMapper;
using HBlog.Application.Automapper;
using HBlog.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace HBlog.TestUtilities
{
    public abstract class TestBase
    {
        protected readonly IMapper _mapper;
        private IConfiguration _configuration;
        private static readonly ServiceProvider _serviceProvider;
        private static readonly ResourceManager _resourceManager;

        public TestBase() 
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutoMapperProfiles());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            var inMemorySettings = new Dictionary<string, string> {
                //{"TopLevelKey", "TopLevelValue"},
                //{"SectionName:SomeKey", "SectionValue"},
                // Need to set up the 
            };

            _configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

        }

        protected static T GetService<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public DefaultHttpContext UserSetup()
        {
            var context = new DefaultHttpContext();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "kevin0"),
                new Claim(ClaimTypes.Name, "kevin0"),

            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            context.User = claimsPrincipal;
            return context;
        }
    }
}
