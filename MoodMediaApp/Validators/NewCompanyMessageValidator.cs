using FluentValidation;
using MoodMediaApp.Messages;

namespace MoodMediaApp.Validators;

public class NewCompanyMessageValidator : AbstractValidator<NewCompanyMessage>
{
    public NewCompanyMessageValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required.")
            .Length(1, 255).WithMessage("Company name must be between 1 and 255 characters long.");

        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("Company code is required.")
            .Length(1, 50).WithMessage("Company code must be between 1 and 50 characters.");

        RuleFor(x => x.Licensing)
            .IsInEnum().WithMessage("Licensing type is invalid.");

        RuleFor(x => x.Devices)
            .NotNull().WithMessage("Devices cannot be null.")
            .DependentRules(() =>
            {
                RuleFor(x => x.Devices)
                    .Must(d => d.Any()).WithMessage("Device list cannot be empty.");

                RuleForEach(x => x.Devices)
                    .NotNull().WithMessage("Device cannot be null.")
                    .SetValidator(new DeviceMessageValidator());
            });

    }
}
