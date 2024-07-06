using FluentValidation;
using MoodMediaApp.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMediaApp.Validators;

public class DeviceMessageValidator : AbstractValidator<DeviceMessage>
{
    public DeviceMessageValidator()
    {
        RuleFor(x => x.OrderNo)
            .NotEmpty().WithMessage("Order number is required.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Device type is invalid.");
    }
}