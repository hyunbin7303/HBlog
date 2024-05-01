using HBlog.Domain.Entities;

namespace HBlog.Domain.Repositories
{
    public interface IGroupRepository
    {
        void AddGroup(Group group);
        Task<Group> GetMsgGroup(string groupName);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);
    }
}