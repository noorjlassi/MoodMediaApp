using MoodMediaApp.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMediaApp.Interfaces;

public interface ICompanyService
{
    Task<MessageResult> CreateCompany(NewCompanyMessage message);
}