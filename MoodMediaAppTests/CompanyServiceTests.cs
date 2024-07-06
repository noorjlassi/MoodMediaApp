using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MoodMediaApp.Data.Commands;
using MoodMediaApp.Data.Queries;
using MoodMediaApp.Enums;
using MoodMediaApp.Messages;
using MoodMediaApp.Services;
using Moq;
using MoodMediaApp.Interfaces;

namespace MoodMediaAppTests
{
    public class CompanyServiceTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IDatabaseOperation> _mockDatabaseService;
        private readonly Mock<IValidator<NewCompanyMessage>> _mockValidator;
        private readonly CompanyService _service;

        public CompanyServiceTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockDatabaseService = new Mock<IDatabaseOperation>();
            _mockValidator = new Mock<IValidator<NewCompanyMessage>>();

            _mockDatabaseService.Setup(m => m.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _mockDatabaseService.Setup(m => m.CommitTransactionAsync()).Returns(Task.CompletedTask);
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<NewCompanyMessage>(), default))
                          .ReturnsAsync(new ValidationResult());

            _service = new CompanyService(_mockMediator.Object, _mockDatabaseService.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task CreateCompany_ValidData_ReturnsSuccess()
        {
            var deviceMessage = new DeviceMessage
            {
                Type = DeviceType.Custom,
                OrderNo = "Order11"
            };

            var message = new NewCompanyMessage
            {
                CompanyName = "New Co",
                CompanyCode = "NC123",
                Devices = new List<DeviceMessage> { deviceMessage },
                Licensing = (LicensingType)1
            };

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<NewCompanyMessage>(), default))
                          .ReturnsAsync(new ValidationResult());

            _mockMediator.Setup(x => x.Send(It.IsAny<CompanyExistsQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            _mockMediator.Setup(x => x.Send(It.IsAny<AddCompanyCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1); 
            
            var result = await _service.CreateCompany(message);

            Assert.True(result.Success);
            Assert.Equal("Company created successfully.", result.Message);
        }

        [Fact]
        public async Task CreateCompany_InvalidData_ReturnsFailure()
        {
            var message = new NewCompanyMessage
            {
                CompanyName = "", 
                CompanyCode = "NC123",
                Devices = new List<DeviceMessage>(),
                Licensing = (LicensingType)1
            };

            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("CompanyName", "Company name is required.")
            };
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<NewCompanyMessage>(), default))
                          .ReturnsAsync(new ValidationResult(failures));

            var result = await _service.CreateCompany(message);

            Assert.False(result.Success);
            Assert.Contains("Company name is required", result.Message);
        }

        [Fact]
        public async Task CreateCompany_WhenCompanyExists_ReturnsFailure()
        {
            _mockMediator.Setup(x => x.Send(It.IsAny<CompanyExistsQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var message = new NewCompanyMessage
            {
                CompanyName = "Existing Co",
                CompanyCode = "EX123"
            };

            var result = await _service.CreateCompany(message);

            Assert.False(result.Success);
            Assert.Equal("A company with the provided code already exists.", result.Message);
        }

        [Fact]
        public async Task CreateCompany_ThrowsException_ReturnsFailure()
        {
            _mockMediator.Setup(x => x.Send(It.IsAny<CompanyExistsQuery>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Database error"));

            var message = new NewCompanyMessage
            {
                CompanyName = "Test Co",
                CompanyCode = "TC123",
                Devices = new List<DeviceMessage> { new DeviceMessage { OrderNo = "Order12", Type = DeviceType.Custom } },
                Licensing = (LicensingType)1
            };

            var result = await _service.CreateCompany(message);

            Assert.False(result.Success);
            Assert.Equal("An error occurred: Database error", result.Message);
        }

    }
}
