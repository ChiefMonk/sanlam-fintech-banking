using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Sanlam.Chipo.Bank.Domain.Options;

/// <summary>
///  Config Options for Redis
/// </summary>
/// <seealso cref="System.IEquatable&lt;Sanlam.Chipo.Bank.Domain.Options.RedisSettingOptions&gt;" />
[ExcludeFromCodeCoverage]
public record RedisSettingOptions
{
    internal const string ConfigName = "SanlamBank:Redis";

    [JsonPropertyName("ConnectionString"), Required]
    public required string ConnectionString { get; init; }

    [JsonPropertyName("Options"), Required, ValidateObjectMembers]
    public required RedisSettings? Options { get; init; }

    /// <summary>
    /// Returns true if ... is valid.
    /// </summary>
    /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
    /// <returns>
    ///   <c>true</c> if the specified throw exception is valid; otherwise, <c>false</c>.
    /// </returns>
    public bool IsValid(bool throwException = true)
    {
        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            return throwException 
                ? throw new ArgumentNullException(ConnectionString) 
                : false;
        }

        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            return throwException
                ? throw new ArgumentNullException(ConnectionString)
                : false;
        }

        return true; 
    }
}

[ExcludeFromCodeCoverage]
public sealed record RedisSettings
{
    [JsonPropertyName("BreakInMs"), Required]
    public required int BreakInMs { get; init; }

    [JsonPropertyName("SleepInMs"), Required]
    public required int SleepInMs { get; init; }

    [JsonPropertyName("ExceptionsBeforeBreaking"), Required]
    public required int ExceptionsBeforeBreaking { get; init; }

    [JsonPropertyName("RetryCount"), Required]
    public required int RetryCount { get; init; }
}
