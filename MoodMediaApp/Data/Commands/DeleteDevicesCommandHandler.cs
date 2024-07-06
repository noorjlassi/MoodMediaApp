using Dapper;
using MediatR;
using MoodMediaApp.Interfaces;
using System.Text;

namespace MoodMediaApp.Data.Commands;

public class DeleteDevicesCommandHandler : IRequestHandler<DeleteDevicesCommand, int>
{
    private readonly IDatabaseOperation _databaseService;

    public DeleteDevicesCommandHandler(IDatabaseOperation databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<int> Handle(DeleteDevicesCommand request, CancellationToken cancellationToken)
    {
        if (request.SerialNumbers == null || request.SerialNumbers.Count == 0)
            return 0;

        var parameters = new DynamicParameters();
        var inClause = new StringBuilder();
        for (int i = 0; i < request.SerialNumbers.Count; i++)
        {
            var paramName = $"@SerialNumber{i}";
            inClause.Append(paramName);
            if (i < request.SerialNumbers.Count - 1)
                inClause.Append(", ");
            parameters.Add(paramName, request.SerialNumbers[i]);
        }

        var query = $"DELETE FROM Device WHERE SerialNumber IN ({inClause})";
        int deletedCount = await _databaseService.ExecuteCommandAsync(query, parameters);
        return deletedCount;
    }
}
