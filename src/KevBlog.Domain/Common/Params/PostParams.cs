namespace KevBlog.Domain.Common.Params
{
    public class PostParams : QueryParams
    {
        public List<int> TagIds { get; set; } = new List<int>();
        public int CategoryId { get; set; }
    }
}
