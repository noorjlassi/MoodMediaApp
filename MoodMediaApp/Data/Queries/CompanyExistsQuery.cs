using MediatR;

namespace MoodMediaApp.Data.Queries;

public class CompanyExistsQuery : IRequest<bool>
{
    public string CompanyCode { get; set; }
}
