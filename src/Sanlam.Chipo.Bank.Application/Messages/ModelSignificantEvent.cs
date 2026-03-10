namespace Sanlam.Chipo.Bank.Application.Messages;

/// <summary>
///  Model for SignificantEvent
/// </summary>
public record ModelSignificantEvent(
    DateTime EventTime,
    string EventName,
    string SourceName,
    string Message);