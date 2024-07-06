using MediatR;
using MoodMediaApp.Interfaces;

namespace MoodMediaApp.Data.Commands;

public class AddLocationCommandHandler : IRequestHandler<AddLocationCommand, int>
{
    private readonly IDatabaseOperation _databaseService;

    public AddLocationCommandHandler(IDatabaseOperation databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<int> Handle(AddLocationCommand request, CancellationToken cancellationToken)
    {
        var query = "INSERT INTO Location (Name, Address, ParentId) OUTPUT INSERTED.ID VALUES (@Name, @Address, @ParentId)";
        return await _databaseService.ExecuteScalarAsync(query, new { request.Name, request.Address, request.ParentId });
    }
}