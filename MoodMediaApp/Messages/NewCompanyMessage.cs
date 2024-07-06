using FluentValidation;
using MoodMediaApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMediaApp.Messages;

public class NewCompanyMessage : Message
{
    public string CompanyName { get; set; }
    public string CompanyCode { get; set; }
    public LicensingType Licensing { get; set; }
    public List<DeviceMessage> Devices { get; set; }
}

public class DeviceMessage
{
    public string OrderNo { get; set; }
    public DeviceType Type { get; set; }
}