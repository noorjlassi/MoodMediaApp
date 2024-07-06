using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMediaApp.Messages;

public class MessageResult
{
    public bool Success { get; set; }
    public string Message { get; set; }

    public static MessageResult Ok(string message = "") =>
        new() { Success = true, Message = message };

    public static MessageResult Fail(string message) =>
        new() { Success = false, Message = message };
}