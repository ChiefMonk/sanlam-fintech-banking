using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Sanlam.Chipo.Bank.Domain.Options;

[ExcludeFromCodeCoverage]
public record SqlConnectionsOptions
{
    internal const string ConfigName = "SanlamBank:Sql";

    [JsonPropertyName("BankConnection"), Required]
    public required string BankConnection { get; init; }

    [JsonPropertyName("OtherConnection"), Required]
    public required string OtherConnection { get; init; }
    public bool IsValid(bool throwException = true)
    {
        if (string.IsNullOrWhiteSpace(BankConnection))
        {
            return throwException 
                ? throw new ArgumentNullException(BankConnection) 
                : false;
        }

        if (string.IsNullOrWhiteSpace(OtherConnection))
        {
            return throwException
                ? throw new ArgumentNullException(OtherConnection)
                : false;
        }

        return true; 
    }
}