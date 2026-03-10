using Sanlam.Chipo.Bank.Domain.Enums;
using System.Text.Json.Serialization;

namespace Sanlam.Chipo.Bank.Domain.Result;

public sealed record SystemActionError(
    [property: JsonIgnore] int StatusCode,
    [property: JsonPropertyOrder(1),
               JsonPropertyName("result_code")]
    BankAccountActionResult ResultCode,
    [property: JsonPropertyOrder(2),
               JsonPropertyName("result_description")]
    string ResultDescription)
{
    public static readonly SystemActionError None = new SystemActionError(
        200,
        BankAccountActionResult.Successful,
        string.Empty);

    public static readonly SystemActionError NullValue = new SystemActionError(
        403,
        BankAccountActionResult.UnknownError,
        "Null value was provided");
}