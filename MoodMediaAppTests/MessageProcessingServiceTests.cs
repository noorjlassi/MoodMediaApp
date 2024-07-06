using MoodMediaApp.Interfaces;
using MoodMediaApp.Messages;
using MoodMediaApp.Services;
using Moq;
using Newtonsoft.Json;

namespace MoodMediaAppTests
{
    public class MessageProcessingServiceTests
    {
        private readonly Mock<ILoggerService> _mockLoggerService;
        private readonly Mock<ICompanyService> _mockCompanyService;
        private readonly Mock<IDeviceService> _mockDeviceService;
        private readonly MessageProcessingService _service;

        public MessageProcessingServiceTests()
        {
            _mockLoggerService = new Mock<ILoggerService>();
            _mockCompanyService = new Mock<ICompanyService>();
            _mockDeviceService = new Mock<IDeviceService>();
            _service = new MessageProcessingService(_mockLoggerService.Object, _mockCompanyService.Object, 
                _mockDeviceService.Object);
        }

        [Fact]
        public async Task ProcessMessageAsync_InvalidJson_ThrowsJsonReaderException()
        {
            string jsonInput = "invalid json";

            await Assert.ThrowsAsync<JsonReaderException>(() => _service.ProcessMessageAsync(jsonInput));
        }

        [Fact]
        public async Task ProcessMessageAsync_UnknownMessageType_ThrowsInvalidOperationException()
        {
            string jsonInput = "{\"MessageType\":\"UnknownType\"}";

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ProcessMessageAsync(jsonInput));
        }

    }
}
