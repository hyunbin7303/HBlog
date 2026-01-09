namespace HBlog.Domain.Common.Exceptions;

/// <summary>
/// Exception thrown when domain rules are violated
/// </summary>
public class DomainException : Exception
{
    public List<string> Errors { get; }

    public DomainException(string message) : base(message)
    {
        Errors = new List<string> { message };
    }

    public DomainException(string message, Exception innerException) 
        : base(message, innerException)
    {
        Errors = new List<string> { message };
    }

    public DomainException(List<string> errors) 
        : base(string.Join("; ", errors))
    {
        Errors = errors;
    }

    public static DomainException ValidationFailed(string message) 
        => new($"Validation failed: {message}");

    public static DomainException InvalidOperation(string message) 
        => new($"Invalid operation: {message}");
}