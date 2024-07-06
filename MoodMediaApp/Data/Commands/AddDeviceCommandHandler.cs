using MediatR;
using MoodMediaApp.Interfaces;

namespace MoodMediaApp.Data.Commands;

public class AddDeviceCommandHandler : IRequestHandler<AddDeviceCommand, Unit>
{
    private readonly IDatabaseOperation _databaseService;

    public AddDeviceCommandHandler(IDatabaseOperation databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<Unit> Handle(AddDeviceCommand request, CancellationToken cancellationToken)
    {
        var query = "INSERT INTO Device (SerialNumber, Type, LocationId) VALUES (@SerialNumber, @Type, @LocationId)";
        await _databaseService.ExecuteCommandAsync(query, new { request.SerialNumber, Type = (int)request.Type, request.LocationId });
        return Unit.Value;
    }
}