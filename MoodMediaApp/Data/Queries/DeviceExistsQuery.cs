using MediatR;

namespace MoodMediaApp.Data.Queries;

public class DeviceExistsQuery : IRequest<bool>
{
    public string SerialNumber { get; set; }
}
