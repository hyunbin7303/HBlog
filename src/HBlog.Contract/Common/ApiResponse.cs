namespace HBlog.Contract.Common
{
    public record ApiResponse(bool Success = true, string ErrorMessage = null);
    public record ApiResponse<T>(T Data, bool Success = true, string ErrorMessage = null);
}
