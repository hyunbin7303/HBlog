namespace HBlog.Domain.Common
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }
    }
}
