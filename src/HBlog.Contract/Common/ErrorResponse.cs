using System.Data.Common;
using System.Net;
using System.Text.Json;

namespace HBlog.Contract.Common
{
    public class ErrorResponse : IResult
    {
        private readonly Exception _exception;

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public int StatusCode { get; set; } 

        public ErrorResponse(Exception e)
        {
            var properties = GetProperties(e);
            IsSuccess = false;
            Errors = new List<string>() { properties.ErrorMessage };
            StatusCode = properties.StatusCode;
        }

        public async Task<string> ToJson()
        {
            return await Task.Run(()=>JsonSerializer.Serialize(this));
        }

        private static Properties GetProperties(Exception e)
        {
            string errorMessage = string.Empty;
            int statusCode = (int)HttpStatusCode.InternalServerError;

            switch (e)
            {
                case DbException:
                    {
                        errorMessage = $"SQL server error - {e.Message}";
                        statusCode = 510;
                        break;
                    }
                case HttpRequestException:
                    {
                        errorMessage = $"Bad Request - {e.Message}";
                        statusCode = (int)HttpStatusCode.Forbidden; // Forbidden
                        break;
                    }
                default:
                    {
                        errorMessage = e.Message;
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                    }
            }

            return new Properties(errorMessage, statusCode);

        }

        private class Properties(string errorMessage, int statusCode)
        {
            public string ErrorMessage { get; } = errorMessage;
            public int StatusCode { get; } = statusCode;
        }
    }
}
