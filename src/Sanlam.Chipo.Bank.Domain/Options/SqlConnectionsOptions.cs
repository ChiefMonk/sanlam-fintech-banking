using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Sanlam.Chipo.Bank.Domain.Options;

/// <summary>
/// Config Options for SqlConnections
/// </summary>
/// <seealso cref="System.IEquatable&lt;Sanlam.Chipo.Bank.Domain.Options.SqlConnectionsOptions&gt;" />
[ExcludeFromCodeCoverage]
public record SqlConnectionsOptions
{
    internal const string ConfigName = "SanlamBank:Sql";

    [JsonPropertyName("BankConnection"), Required]
    public required string BankConnection { get; init; }

    [JsonPropertyName("OtherConnection"), Required]
    public required string OtherConnection { get; init; }
    /// <summary>
    /// Returns true if ... is valid.
    /// </summary>
    /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
    /// <returns>
    ///   <c>true</c> if the specified throw exception is valid; otherwise, <c>false</c>.
    /// </returns>
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