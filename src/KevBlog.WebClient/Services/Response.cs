namespace KevBlog.WebClient.Services
{
    public class Response<T>
    {
        public string Message { get; set; }
        public string ValidationError { get; set; }
        public bool IsSuccess { get; set; }
        public T Dasta { get; set; }
    }
}
