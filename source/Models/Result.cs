namespace URLShortener.Models;

// Create a generic result object to handle the success/failure of our service instead of throwing exceptions.
public class Result<T>
{
    public bool IsSuccessful { get; private set; }
    public T? Data { get; private set; }
    public string ErrorMessage { get; private set; }
    
    public static Result<T> Success(T? data = default) => new () { Data = data, IsSuccessful =  true };
    public static Result<T> Failure(string errorMessage) => new () { ErrorMessage =  errorMessage };
}