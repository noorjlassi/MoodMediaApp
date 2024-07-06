using MediatR;

namespace MoodMediaApp.Data.Commands;

public class AddLogEntryCommand : IRequest<Unit>
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string LogLevel { get; set; }
    public string Message { get; set; }
    public string? Exception { get; set; }
}