namespace KevBlog.Application.Common
{
    public interface IResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
