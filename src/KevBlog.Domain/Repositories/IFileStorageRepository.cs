using KevBlog.Domain.Entities;
namespace KevBlog.Domain.Repositories
{
    public interface IFileStorageRepository
    {
        Task<FileStorage> GetbyIdAsync(string id);
        Task<FileStorage> CreateNewStorage(string bucketName);
    }
}