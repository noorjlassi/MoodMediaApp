using MediatR;
using MoodMediaApp.Data.Commands;
using MoodMediaApp.Data.Queries;
using MoodMediaApp.Messages;
using MoodMediaApp.Utilities;
using FluentValidation;
using MoodMediaApp.Interfaces;

namespace MoodMediaApp.Services;

public class CompanyService : ICompanyService
{
    private readonly IMediator _mediator;
    private readonly IDatabaseOperation _databaseService;
    private readonly IValidator<NewCompanyMessage> _validator;

    public CompanyService(IMediator mediator, IDatabaseOperation databaseService, IValidator<NewCompanyMessage> validator)
    {
        _mediator = mediator;
        _databaseService = databaseService;
        _validator = validator;
    }

    public async Task<MessageResult> CreateCompany(NewCompanyMessage message)
    {
        
        var validationResults = await _validator.ValidateAsync(message);
        if (!validationResults.IsValid)
        {
            return MessageResult.Fail(validationResults.Errors.Select(e => e.ErrorMessage).Aggregate((i, j) => i + ", " + j));
        }

        await _databaseService.BeginTransactionAsync();
        bool shouldRollback = true;

        try
        {
            bool companyExists = await _mediator.Send(new CompanyExistsQuery { CompanyCode = message.CompanyCode });
            if (companyExists)
            {
                return MessageResult.Fail("A company with the provided code already exists.");
            }

            var companyId = await _mediator.Send(new AddCompanyCommand
            {
                Name = message.CompanyName,
                Code = message.CompanyCode,
                Licensing = message.Licensing
            });

            for (var i = 0; i < message.Devices.Count; i++)
            {
                string serialNumber = SerialNumberUtility.GenerateSerialNumber();
                bool deviceExists = await _mediator.Send(new DeviceExistsQuery { SerialNumber = serialNumber });
                if (deviceExists)
                {
                    return MessageResult.Fail("A device with the generated serial number already exists.");
                }

                await _mediator.Send(new AddDeviceCommand
                {
                    SerialNumber = serialNumber,
                    Type = message.Devices[i].Type,
                    LocationId = await _mediator.Send(new AddLocationCommand
                    {
                        Name = $"Location {i + 1}",
                        Address = $"Location Address {i + 1}",
                        ParentId = companyId
                    })
                });
            }
            shouldRollback = false;
        }

        catch (Exception ex)
        {
            return MessageResult.Fail($"An error occurred: {ex.Message}");
        }

        finally
        {
            if (shouldRollback)
                await _databaseService.RollbackTransactionAsync();
            else
                await _databaseService.CommitTransactionAsync();
        }

        return MessageResult.Ok("Company created successfully.");
    }


}
