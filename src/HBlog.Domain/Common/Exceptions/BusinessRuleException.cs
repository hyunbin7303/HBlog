namespace HBlog.Domain.Common.Exceptions
{
    public class BusinessRuleException(IBusinessRule brokenRule) : Exception(brokenRule.Message)
    {
	    public IBusinessRule BrokenRule { get; } = brokenRule;

	    public string Details { get; } = brokenRule.Message;

	    public override string ToString()
	    {
		    return $"{BrokenRule.GetType().FullName}: {BrokenRule.Message}";
	    }
	}
}
