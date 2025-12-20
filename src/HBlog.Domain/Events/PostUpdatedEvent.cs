using HBlog.Domain.Common;

namespace HBlog.Domain.Events
{
    public record PostUpdatedEvent(int PostId) : DomainEvent;
}