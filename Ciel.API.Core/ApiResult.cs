namespace Ciel.API.Core;

public class ApiResult<T>
{
    public ApiResult() { }

    public ApiResult(bool succeeded, T result, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Result = result;
        Errors = errors;
    }

    public bool Succeeded { get; set; }

    public T Result { get; set; }

    public IEnumerable<string> Errors { get; set; }

    public static ApiResult<T> Success(T result) => new(true, result, new List<string>());

    public static ApiResult<T> Failure(IEnumerable<string> errors) => new(false, default, errors);
}