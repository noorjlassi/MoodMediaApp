using MediatR;
using MoodMediaApp.Interfaces;

namespace MoodMediaApp.Data.Commands;

public class AddLogEntryCommandHandler : IRequestHandler<AddLogEntryCommand, Unit>
{
    private readonly IDatabaseOperation _databaseService;

    public AddLogEntryCommandHandler(IDatabaseOperation databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<Unit> Handle(AddLogEntryCommand request, CancellationToken cancellationToken)
    {
        var query = @"
            INSERT INTO ApplicationLogs (Timestamp, LogLevel, Message, Exception)
            VALUES (@Timestamp, @LogLevel, @Message, @Exception)";

        await _databaseService.ExecuteCommandAsync(query, new
        {
            request.Timestamp,
            request.LogLevel,
            request.Message,
            request.Exception
        });

        return Unit.Value;
    }
}
