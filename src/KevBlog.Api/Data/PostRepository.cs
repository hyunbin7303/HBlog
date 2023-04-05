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

        public async Task<Post> GetPostById(int id)
        {
            return await _dbContext.Posts.Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async Task<Post> GetPostByUsername(string username)
        {
            var test = await _dbContext.Posts.Where(x => x.User.UserName == username).SingleOrDefaultAsync();
            return test;
        }

        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            return await _dbContext.Posts.ToListAsync();
        }

        public void Remove(int id)
        {
            _dbContext.Posts.Remove(new Post { Id = id });
        }

        public async Task<bool> SaveAllAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(Post user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
        }
    }
}