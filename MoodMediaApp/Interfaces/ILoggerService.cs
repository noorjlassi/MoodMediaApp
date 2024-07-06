using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMediaApp.Interfaces;

public interface ILoggerService
{
    Task LogAsync(string message, string logLevel, string? exception = null);
}

