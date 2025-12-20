using HBlog.Domain.Common;
using HBlog.Domain.Entities;

namespace HBlog.Domain.Events
{
    public record PostCreatedEvent(Post Post) : DomainEvent;
}