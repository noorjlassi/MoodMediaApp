using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMediaApp.Utilities;

public static class SerialNumberUtility
{
    public static string GenerateSerialNumber()
    {
        return Guid.NewGuid().ToString();
    }
}
