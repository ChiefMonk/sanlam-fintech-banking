using System.Diagnostics.CodeAnalysis;

namespace Sanlam.Chipo.Bank.Domain.Result;

public record SystemActionResult
{
    public SystemActionResult(bool isSuccess, SystemActionError error)
    {
        if (isSuccess && error != SystemActionError.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == SystemActionError.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

	public SystemActionError Error { get; }

    public static SystemActionResult Success() => 
	    new(true, SystemActionError.None);

    public static SystemActionResult<TValue> Success<TValue>(TValue value) => 
	    new(value, true, SystemActionError.None);

    public static SystemActionResult Failure(SystemActionError error) => 
	    new(false, error);

    public static SystemActionResult<TValue> Failure<TValue>(SystemActionError error) =>
	    new(default, false, error);

    public static SystemActionResult<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(SystemActionError.NullValue);
}

public sealed record SystemActionResult<TValue> : SystemActionResult
{
    private readonly TValue? _value;

    public SystemActionResult(TValue? value, bool isSuccess, SystemActionError error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator SystemActionResult<TValue>(TValue? value)
        => Create(value);
}