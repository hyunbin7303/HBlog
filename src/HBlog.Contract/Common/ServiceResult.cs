namespace HBlog.Contract.Common;
public struct ServiceResult : IResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }

    public ServiceResult(bool isSuccess, string message, List<string> errors)
    {
        if (message == "")
            message = isSuccess ? "Success to return from service layer." : "Failed to return from service layer.";

        IsSuccess = isSuccess;
        Message = message;
        Errors = errors ?? new List<string>();
    }

    public static ServiceResult Success(string msg = "") => new ServiceResult(true, msg, default);
    public static ServiceResult<T> Success<T>(T value = default, string msg = "") => new ServiceResult<T>(true, msg, value, default);
    public static ServiceResult Fail(List<string> errors = default, string msg = "") => new ServiceResult(false, msg, errors);
    public static ServiceResult<T> Fail<T>(List<string> errors = default, string msg = "") => new ServiceResult<T>(false, msg, default, errors);
    public static ServiceResult NotFound(string msg = "") => new ServiceResult(false, "NotFound", default);
    public static ServiceResult NotFound<T>(string msg = "") => new ServiceResult(false, "NotFound", default);
}
public struct ServiceResult<T> : IResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
    public T Value { get; set; }
    public ServiceResult(bool isSuccess, string message, T value, List<string> errors)
    {
        if (message == "")
            message = isSuccess ? "Success for the operation" : "Failed this operation";

        IsSuccess = isSuccess;
        Message = message;
        Value = value;
        Errors = errors ?? new List<string>();
    }
}
                                