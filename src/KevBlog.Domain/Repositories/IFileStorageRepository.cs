using KevBlog.Domain.Entities;
namespace KevBlog.Domain.Repositories
{
    public interface IFileStorageRepository
    {
        Task<FileStorage> GetbyIdAsync(string id);
        Task<bool> InsertDataAsync(string bucketName, string fileName, Stream fileStream);
        Task<FileStorage> CreateNewStorage(string bucketName);
    }
}