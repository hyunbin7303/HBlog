using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        public PostRepository(DataContext dbContext, IMapper mapper)
        {
            this._mapper = mapper;
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Post> GetPostByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            return await _dbContext.Posts.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(Post user)
        {
            throw new NotImplementedException();
        }
    }
}