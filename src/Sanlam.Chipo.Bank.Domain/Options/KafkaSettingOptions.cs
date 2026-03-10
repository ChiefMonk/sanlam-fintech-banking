using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Sanlam.Chipo.Bank.Domain.Options;

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