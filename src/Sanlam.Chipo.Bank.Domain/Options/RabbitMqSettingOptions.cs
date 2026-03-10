using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Sanlam.Chipo.Bank.Domain.Options;

[ExcludeFromCodeCoverage]
public record RabbitMqSettingOptions
{
    internal const string ConfigName = "SanlamBank:RabbitMQ";

    [JsonPropertyName("Hostname"), Required]
    public required string Hostname { get; init; }

    [JsonPropertyName("Username"), Required]
    public required string Username { get; init; }

    [JsonPropertyName("Password"), Required]
    public required string Password { get; init; }

    [JsonPropertyName("ExchangeSignificantEvents"), Required]
    public required string ExchangeSignificantEvents { get; init; }

    [JsonPropertyName("RoutingKeySignificantEvents"), Required]
    public required string RoutingKeySignificantEvents { get; init; }

    [JsonPropertyName("ExchangeWithdrawalEvents"), Required]
    public required string ExchangeWithdrawalEvents { get; init; }

    [JsonPropertyName("RoutingKeyWithdrawalEvents"), Required]
    public required string RoutingKeyWithdrawalEvents { get; init; }

    [JsonPropertyName("ExchangeOtherUsefulEvents"), Required]
    public required string ExchangeOtherUsefulEvents { get; init; }

    [JsonPropertyName("RoutingKeyOtherUsefulEvents"), Required]
    public required string RoutingKeyOtherUsefulEvents { get; init; }

    public bool IsValid(bool throwException = true)
    {
        if (string.IsNullOrWhiteSpace(Hostname))
        {
            return throwException 
                ? throw new ArgumentNullException(Hostname) 
                : false;
        }

        if (string.IsNullOrWhiteSpace(Username))
        {
            return throwException
                ? throw new ArgumentNullException(Username)
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