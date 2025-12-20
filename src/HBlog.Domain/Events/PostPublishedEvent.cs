using HBlog.Domain.Common;

namespace HBlog.Domain.Events
{
    public record PostPublishedEvent(int PostId) : DomainEvent;
}