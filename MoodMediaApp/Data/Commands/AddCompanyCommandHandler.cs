using MediatR;
using MoodMediaApp.Interfaces;

namespace MoodMediaApp.Data.Commands;

public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, int>
{
    private readonly IDatabaseOperation _databaseService;

    public AddCompanyCommandHandler(IDatabaseOperation databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<int> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
    {
        var query = "INSERT INTO Company (Name, Code, Licensing) OUTPUT INSERTED.ID VALUES (@Name, @Code, @Licensing)";
        return await _databaseService.ExecuteScalarAsync(query, new { request.Name, request.Code, Licensing = (int)request.Licensing });
    }
}
