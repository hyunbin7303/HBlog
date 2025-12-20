namespace HBlog.Domain.Common;
public struct Result : IResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }

    public Result(bool isSuccess, string message, List<string>? errors)
    {
        if (message == "")
            message = isSuccess ? "Success to return from service layer." : "Failed to return from service layer.";

        IsSuccess = isSuccess;
        Message = message;
        Errors = errors ?? new List<string>();
    }

    public static Result Success(string msg = "") => new(true, msg, default);
    public static Result<T> Success<T>(T? value = default, string msg = "") => new(true, msg, value, default);
    public static Result Fail(List<string> errors = default, string msg = "") => new(false, msg, errors);
    public static Result<T> Fail<T>(List<string> errors = default, string msg = "") => new(false, msg, default, errors);
    public static Result NotFound(string msg = "") => new(false, "NotFound", default);
    public static Result NotFound<T>(string msg = "") => new(false, "NotFound", default);
}
public struct Result<T> : IResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
    public T Value { get; set; }
    public Result(bool isSuccess, string message, T value, List<string>? errors)
    {
        if (message == "")
            message = isSuccess ? "Success for the operation" : "Failed this operation";

        IsSuccess = isSuccess;
        Message = message;
        Value = value;
        Errors = errors ?? new List<string>();
    }
}
                                