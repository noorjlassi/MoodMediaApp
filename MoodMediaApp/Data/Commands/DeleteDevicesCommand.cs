using MediatR;

namespace MoodMediaApp.Data.Commands;

public class DeleteDevicesCommand : IRequest<int>
{
    public List<string> SerialNumbers { get; set; }
}