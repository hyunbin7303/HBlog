using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBlog.Domain.Common
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }
        
        private readonly List<DomainEvent> _domainEvents = new();
        
        [NotMapped]
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
