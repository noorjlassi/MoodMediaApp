﻿using FluentValidation;
using MoodMediaApp.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMediaApp.Validators;

public class DeleteDevicesMessageValidator : AbstractValidator<DeleteDevicesMessage>
{
    public DeleteDevicesMessageValidator()
    {
        RuleFor(x => x.SerialNumbers)
        .NotNull().WithMessage("SerialNumbers cannot be null.")
        .DependentRules(() =>
        {
             RuleFor(x => x.SerialNumbers)
                 .Must(d => d.Any()).WithMessage("SerialNumbers list cannot be empty.");
        });
    }
}