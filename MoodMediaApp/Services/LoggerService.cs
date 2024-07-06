using MediatR;
using MoodMediaApp.Data.Commands;
using MoodMediaApp.Interfaces;

namespace MoodMediaApp.Services;

public class LoggerService : ILoggerService
{
    private readonly IMediator _mediator;
    private readonly IDatabaseOperation _databaseService;

    public LoggerService(IMediator mediator, IDatabaseOperation databaseService)
    {
        _mediator = mediator;
        _databaseService = databaseService;
    }

    public async Task LogAsync(string message, string logLevel, string? exception = null)
    {
        try
        {
            Console.WriteLine(message);
            await _databaseService.BeginTransactionAsync();
            var logEntryCommand = new AddLogEntryCommand
            {
                Message = message,
                LogLevel = logLevel,
                Exception = exception
            };
            await _mediator.Send(logEntryCommand);
            await _databaseService.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await _databaseService.RollbackTransactionAsync();
            Console.WriteLine($"Couldn't save the logs to database , Exception : {ex.Message}");

        }
    }
}