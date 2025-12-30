namespace HBlog.Domain.Common;

public class EntityNotFoundException<T> : Exception
{
	public EntityNotFoundException(string entityName, T entityId)
		: base($"{entityName} with ID {entityId} was not found.") { }
}