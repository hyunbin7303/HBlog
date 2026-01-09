using HBlog.Domain.Common.Exceptions;

namespace HBlog.Api.ProblemDetails
{
    public class BusinessRuleProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
	    public BusinessRuleProblemDetails(BusinessRuleException exception)
	    {
		    Title = "Business rule broken";
		    Status = StatusCodes.Status409Conflict;
		    Detail = exception.Message;
		    Type = "https://somedomain/business-rule-validation-error";
	    }
    }
}
