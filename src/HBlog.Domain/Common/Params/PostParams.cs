namespace HBlog.Domain.Common.Params
{
    public class PostParams : QueryParams
    {
        public List<int> TagId { get; set; } = new List<int>();
        public int CategoryId { get; set; }
    }
}
