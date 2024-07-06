using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMediaApp.Messages;

public class DeleteDevicesMessage : Message
{
    public List<string> SerialNumbers { get; set; }
}
