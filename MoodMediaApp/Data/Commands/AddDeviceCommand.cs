using MediatR;
using MoodMediaApp.Enums;

namespace MoodMediaApp.Data.Commands;

public class AddDeviceCommand : IRequest<Unit>
{
    public string SerialNumber { get; set; }
    public DeviceType Type { get; set; }
    public int LocationId { get; set; }
}
