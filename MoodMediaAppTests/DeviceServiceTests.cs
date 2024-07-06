using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MoodMediaApp.Data.Commands;
using MoodMediaApp.Interfaces;
using MoodMediaApp.Messages;
using MoodMediaApp.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMediaAppTests
{
    public class DeviceServiceTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IDatabaseOperation> _mockDatabaseService;
        private readonly Mock<IValidator<DeleteDevicesMessage>> _mockValidator;
        private readonly DeviceService _service;

        public DeviceServiceTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockDatabaseService = new Mock<IDatabaseOperation>();
            _mockValidator = new Mock<IValidator<DeleteDevicesMessage>>();

            _mockDatabaseService.Setup(m => m.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _mockDatabaseService.Setup(m => m.CommitTransactionAsync()).Returns(Task.CompletedTask);
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<DeleteDevicesMessage>(), default))
                          .ReturnsAsync(new ValidationResult());

            _service = new DeviceService(_mockMediator.Object, _mockDatabaseService.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task DeleteDevices_ValidData_ReturnsSuccess()
        {
            var message = new DeleteDevicesMessage { SerialNumbers = new List<string> { "123", "456" } };

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<DeleteDevicesMessage>(), default))
                          .ReturnsAsync(new ValidationResult());

            _mockMediator.Setup(m => m.Send(It.IsAny<DeleteDevicesCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(2); 

            var result = await _service.DeleteDevices(message);

            Assert.True(result.Success);
            Assert.Equal("2 devices deleted successfully.", result.Message);
        }

        [Fact]
        public async Task DeleteDevices_InvalidData_ReturnsFailure()
        {
            var message = new DeleteDevicesMessage { SerialNumbers = new List<string>() };

            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("SerialNumbers", "Serial numbers cannot be empty.")
            };
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<DeleteDevicesMessage>(), default))
                          .ReturnsAsync(new ValidationResult(failures));

            var result = await _service.DeleteDevices(message);

            Assert.False(result.Success);
            Assert.Equal("Serial numbers cannot be empty.", result.Message);
        }

    }
}
