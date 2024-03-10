using KevBlog.Domain.Params;
namespace KevBlog.Domain.Common.Params
{
    public class PostParams : PaginationParams
    {
        public int CategoryId { get; set; }
        public List<int> TagId { get; set; }  
    }
}
