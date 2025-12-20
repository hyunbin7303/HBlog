namespace HBlog.WebClient.Commons
{
    public record ApiResponse<T>(T Data, bool Success = true, string? ErrorMessage = null);
}
