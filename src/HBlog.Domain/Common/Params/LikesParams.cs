namespace HBlog.Domain.Params
{
    public class LikesParams : PaginationParams
    {
        public Guid UserId { get; set; }
        public string Predicate { get; set; }
        
    }
}