public class ServiceResult<T>
{
    public bool Success { get; private set; }
    public T Data { get; private set; }
    public string ErrorMessage { get; private set; }

    private ServiceResult(bool success, T data, string errorMessage)
    {
        Success = success;
        Data = data;
        ErrorMessage = errorMessage;
    }

    public static ServiceResult<T> SuccessResult(T data)
    {
        return new ServiceResult<T>(true, data, null);
    }

    public static ServiceResult<T> ErrorResult(string errorMessage)
    {
        return new ServiceResult<T>(false, default, errorMessage);
    }
}
