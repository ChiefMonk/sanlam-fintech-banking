using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Sanlam.Chipo.Bank.Domain.Options;

/// <summary>
/// Config Options for Kafka
/// </summary>
/// <seealso cref="System.IEquatable&lt;Sanlam.Chipo.Bank.Domain.Options.KafkaSettingOptions&gt;" />
[ExcludeFromCodeCoverage]
public record KafkaSettingOptions
{
    internal const string ConfigName = "SanlamBank:Kafka";

    [JsonPropertyName("Hostname"), Required]
    public required string Hostname { get; init; }

    [JsonPropertyName("Password"), Required]
    public required string Password { get; init; }

    [JsonPropertyName("TopicSignificantEvents"), Required]
    public required string TopicSignificantEvents { get; init; }

    [JsonPropertyName("TopicWithdrawalEvents"), Required]
    public required string TopicWithdrawalEvents { get; init; }

    [JsonPropertyName("TopicOtherUsefulEvents"), Required]
    public required string TopicOtherUsefulEvents { get; init; }

    /// <summary>
    /// Returns true if ... is valid.
    /// </summary>
    /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
    /// <returns>
    ///   <c>true</c> if the specified throw exception is valid; otherwise, <c>false</c>.
    /// </returns>
    public bool IsValid(bool throwException = true)
    {
        if (string.IsNullOrWhiteSpace(Hostname))
        {
            return throwException 
                ? throw new ArgumentNullException(Hostname) 
                : false;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            return throwException
                ? throw new ArgumentNullException(Password)
                : false;
        }

        return true; 
    }
}