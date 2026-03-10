using System.Diagnostics.CodeAnalysis;

namespace Sanlam.Chipo.Bank.Domain.Result;

/// <summary>
///   Model for public record SystemActionResult

/// </summary>
public record SystemActionResult
{
    /// <summary>Initializes a new instance of the <see cref="SystemActionResult" /> class.</summary>
    /// <param name="isSuccess">if set to <c>true</c> [is success].</param>
    /// <param name="error">The error.</param>
    /// <exception cref="System.InvalidOperationException"></exception>
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

    /// <summary>Successes this instance.</summary>
    /// <returns>
    ///   <br />
    /// </returns>
    public static SystemActionResult Success() => 
	    new(true, SystemActionError.None);

    /// <summary>Successes the specified value.</summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public static SystemActionResult<TValue> Success<TValue>(TValue value) => 
	    new(value, true, SystemActionError.None);

    /// <summary>Failures the specified error.</summary>
    /// <param name="error">The error.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public static SystemActionResult Failure(SystemActionError error) => 
	    new(false, error);

    /// <summary>Failures the specified error.</summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="error">The error.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public static SystemActionResult<TValue> Failure<TValue>(SystemActionError error) =>
	    new(default, false, error);

    /// <summary>Creates the specified value.</summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>
    ///   <br />
    /// </returns>
    public static SystemActionResult<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(SystemActionError.NullValue);
}

public sealed record SystemActionResult<TValue> : SystemActionResult
{
    private readonly TValue? _value;

    /// <summary>Initializes a new instance of the <see cref="SystemActionResult{TValue}" /> class.</summary>
    /// <param name="value">The value.</param>
    /// <param name="isSuccess">if set to <c>true</c> [is success].</param>
    /// <param name="error">The error.</param>
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