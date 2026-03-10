namespace Sanlam.Chipo.Bank.Application.Messages;

public record ModelSignificantEvent(
    DateTime EventTime,
    string EventName,
    string SourceName,
    string Message);