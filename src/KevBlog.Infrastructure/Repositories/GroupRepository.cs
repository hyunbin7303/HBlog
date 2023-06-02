using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KevBlog.Infrastructure.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly DataContext _dbContext;
        public GroupRepository(DataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public void AddGroup(Group group)
        {
            _dbContext.Groups.Add(group);
        }
        public async Task<Group> GetMsgGroup(string groupName)
        {
            return await _dbContext.Groups.Include(x => x.Connections).FirstOrDefaultAsync(x => x.Name == groupName);
        }
        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _dbContext.Connections.FindAsync(connectionId);
        }
        public void RemoveConnection(Connection connection)
        {
            _dbContext.Connections.Remove(connection);
        }
    }
}
