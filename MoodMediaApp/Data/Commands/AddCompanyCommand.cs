using MediatR;
using MoodMediaApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMediaApp.Data.Commands;

public class AddCompanyCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public LicensingType Licensing { get; set; }
}
