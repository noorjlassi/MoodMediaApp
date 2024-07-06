using MediatR;
using MoodMediaApp.Interfaces;

namespace MoodMediaApp.Data.Queries;

public class CompanyExistsHandler : IRequestHandler<CompanyExistsQuery, bool>
{
    private readonly IDatabaseOperation _databaseService;

    public CompanyExistsHandler(IDatabaseOperation databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<bool> Handle(CompanyExistsQuery request, CancellationToken cancellationToken)
    {
        var query = "SELECT COUNT(1) FROM Company WHERE Code = @Code";
        var count = await _databaseService.ExecuteScalarAsync(query, new { Code = request.CompanyCode });
        return count > 0;
    }
}
