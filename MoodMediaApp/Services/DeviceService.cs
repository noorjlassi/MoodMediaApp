using FluentValidation;
using MediatR;
using MoodMediaApp.Data.Commands;
using MoodMediaApp.Interfaces;
using MoodMediaApp.Messages;

namespace MoodMediaApp.Services
{
    public class DeviceService
    {
        private readonly IMediator _mediator;
        private readonly IDatabaseOperation _databaseService;
        private readonly IValidator<DeleteDevicesMessage> _validator;

        public DeviceService(IMediator mediator, IDatabaseOperation databaseService, IValidator<DeleteDevicesMessage> validator)
        {
            _mediator = mediator;
            _databaseService = databaseService;
            _validator = validator;
        }

        public async Task<MessageResult> DeleteDevices(DeleteDevicesMessage message)
        {
            try
            {
                var validationResults = await _validator.ValidateAsync(message);
                if (!validationResults.IsValid)
                {
                    return MessageResult.Fail(validationResults.Errors.Select(e => e.ErrorMessage).Aggregate((i, j) => i + ", " + j));
                }

                await _databaseService.BeginTransactionAsync();
                int deletedCount = await _mediator.Send(new DeleteDevicesCommand { SerialNumbers = message.SerialNumbers });
                await _databaseService.CommitTransactionAsync();
                return MessageResult.Ok($"{deletedCount} devices deleted successfully.");
            }
            catch (Exception ex)
            {
                await _databaseService.RollbackTransactionAsync();
                return MessageResult.Fail($"An error occurred: {ex.Message}");
            }
        }

    }
}
