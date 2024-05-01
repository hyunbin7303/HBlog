using HBlog.Domain.Entities;
namespace HBlog.Domain.Repositories
{
    public interface IFileStorageRepository : IRepository<FileStorage>
    {
        Task<List<FileStorage>> GetAllFilesByUserIdAsync(string userId);
        Task<bool> InsertDataAsync(string bucketName, string fileName, Stream fileStream);
    }
}