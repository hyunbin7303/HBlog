
using AutoMapper;
using KevBlog.Application.Automapper;
using KevBlog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KevBlog.UnitTests
{
    public class TestBase
    {
        protected readonly IMapper _mapper;
        private static readonly ServiceProvider _serviceProvider;
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
        protected User GetUserFake(int fakeUserId)
        {
            return new User()
            {
                Id = fakeUserId,
                UserName = "kevin" + fakeUserId,
                KnownAs = "kevin",
                City = "Kitchener"
            };
        }
    }
}
