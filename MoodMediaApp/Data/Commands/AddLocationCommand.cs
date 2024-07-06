using MediatR;

namespace MoodMediaApp.Data.Commands;

public class AddLocationCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public int ParentId { get; set; }
}