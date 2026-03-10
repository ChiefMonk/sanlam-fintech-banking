using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Sanlam.Chipo.Bank.Domain.Options;

[ExcludeFromCodeCoverage]
public record OpenTelemetryOptions
{
    internal const string ConfigName = "SanlamBank:OpenTelemetry";

    [JsonPropertyName("Url"), Required]
    public required string Url { get; init; }

    [JsonPropertyName("EndpointType"), Required]
    public required string EndpointType { get; init; }

    [JsonPropertyName("SamplingType"), Required]
    public required string SamplingType { get; init; }

    [JsonPropertyName("SamplingRatio"), Required]
    public required double SamplingRatio { get; init; }

    public bool IsValid(bool throwException = true)
    {
        if (string.IsNullOrWhiteSpace(Url))
        {
            return throwException 
                ? throw new ArgumentNullException(Url) 
                : false;
        }

        if (string.IsNullOrWhiteSpace(EndpointType))
        {
            return throwException
                ? throw new ArgumentNullException(EndpointType)
                : false;
        }

        if (string.IsNullOrWhiteSpace(SamplingType))
        {
            return throwException
                ? throw new ArgumentNullException(SamplingType)
                : false;
        }

        if (SamplingRatio == 0)
        {
            return throwException
                ? throw new ArgumentNullException(nameof(SamplingRatio))
                : false;
        }

        return true; 
    }
}