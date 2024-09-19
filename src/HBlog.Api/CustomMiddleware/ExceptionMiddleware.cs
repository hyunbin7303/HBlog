using HBlog.Contract.Common;
using NuGet.Protocol;

namespace HBlog.Api.CustomMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next) 
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }catch(Exception ex)
            {
                // Log Error as Task for non-blocking

                ErrorResponse errorResponse = await HandleExceptionAsync(context,ex);                
                await context.Response.WriteAsync(await errorResponse.ToJson());
            }
        }

        private async Task<ErrorResponse> HandleExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = @"application/json";
            
            ErrorResponse errorResponse = new ErrorResponse(e);

            context.Response.StatusCode = errorResponse.StatusCode;
            return errorResponse;
        }
    }
}
