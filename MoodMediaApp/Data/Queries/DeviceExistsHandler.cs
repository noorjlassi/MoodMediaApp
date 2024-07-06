using MediatR;
using MoodMediaApp.Interfaces;

namespace MoodMediaApp.Data.Queries;

public class DeviceExistsHandler : IRequestHandler<DeviceExistsQuery, bool>
{
    private readonly IDatabaseOperation _databaseService;

    public DeviceExistsHandler(IDatabaseOperation databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<bool> Handle(DeviceExistsQuery request, CancellationToken cancellationToken)
    {
        var query = "SELECT COUNT(1) FROM Device WHERE SerialNumber = @SerialNumber";
        var parameters = new { SerialNumber = request.SerialNumber };
        var count = await _databaseService.ExecuteScalarAsync(query, parameters);
        return count > 0;
    }
}