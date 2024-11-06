using PokedexBackend.Domain.Enums;

namespace PokedexBackend.Domain.Common;

public class OperationResult<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? ErrorMessage { get; }
    public EOperationResultCode Code { get; }

    private OperationResult(T value)
    {
        IsSuccess = true;
        Value = value;
        Code = EOperationResultCode.Success;
        ErrorMessage = null;
    }

    private OperationResult(EOperationResultCode code, string errorMessage)
    {
        IsSuccess = false;
        Value = default; 
        Code = code;
        ErrorMessage = errorMessage;
    }

    public static OperationResult<T> Success(T value) =>
        new OperationResult<T>(value);

    public static OperationResult<T> Failure(EOperationResultCode code, string errorMessage) =>
        new OperationResult<T>(code, errorMessage);
} 