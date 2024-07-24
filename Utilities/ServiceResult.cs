namespace ProgramPlatform.Utilities;

/// <summary>
/// Represents the result of a service operation.
/// </summary>
/// <typeparam name="T">The type of the data returned in the result.</typeparam>
public class ServiceResult<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Represents a successful service result.
    /// </summary>
    /// <typeparam name="T">The type of the data returned in the result.</typeparam>
    /// <param name="data">The data returned in the result.</param>
    /// <returns>A successful service result with the provided data.</returns>
    public static ServiceResult<T> Successful(T data) => new()
    {
        Success = true,
        Data = data
    };

    /// <summary>
    /// Represents a failed service result.
    /// </summary>
    /// <typeparam name="T">The type of the data returned in the result.</typeparam>
    /// <param name="errorMessage">The error message associated with the failed result.</param>
    /// <returns>A failed service result with the provided error message.</returns>
    public static ServiceResult<T> Failure(string errorMessage) => new()
    {
        Success = false,
        ErrorMessage = errorMessage
    };
}