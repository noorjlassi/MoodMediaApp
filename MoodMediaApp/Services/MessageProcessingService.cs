using MoodMediaApp.Interfaces;
using MoodMediaApp.Messages;
using Newtonsoft.Json;

namespace MoodMediaApp.Services;

public class MessageProcessingService
{
    private readonly ILoggerService _loggerService;
    private readonly ICompanyService _companyService;
    private readonly IDeviceService _deviceService;

    public MessageProcessingService(ILoggerService loggerService, ICompanyService companyService, 
        IDeviceService deviceService)
    {
        _loggerService = loggerService;
        _companyService = companyService;
        _deviceService = deviceService;
    }

    public async Task ProcessMessageAsync(string jsonInput)
    {
        await _loggerService.LogAsync($"Received JSON input: {jsonInput}", "Debug");
        var message = JsonConvert.DeserializeObject<Message>(jsonInput);
        if (message == null)
        {
            await _loggerService.LogAsync("Invalid JSON input.", "Error");
            throw new JsonException("Invalid JSON input.");
        }

        switch (message.MessageType)
        {
            case "NewCompany":
                await ProcessNewCompanyMessage(jsonInput);
                break;
            case "DeleteDevices":
                await ProcessDeleteDevicesMessage(jsonInput);
                break;
            default:
                await _loggerService.LogAsync("Received unknown message type", "Warning");
                throw new InvalidOperationException("Unknown MessageType.");
        }
    }

    private async Task ProcessNewCompanyMessage(string jsonInput)
    {
        try
        {
            var newCompanyMessage = JsonConvert.DeserializeObject<NewCompanyMessage>(jsonInput);
            if (newCompanyMessage == null)
            {
                await _loggerService.LogAsync("Failed to deserialize JSON message for NewCompany.", "Error");
                throw new InvalidOperationException("Failed to deserialize the JSON message into a NewCompanyMessage.");
            }
            await _loggerService.LogAsync("Processing NewCompany operation", "Info");
            var result = await _companyService.CreateCompany(newCompanyMessage);
            var resultStatus = result.Success ? "Success" : "Failed";
            await _loggerService.LogAsync($"Operation {resultStatus}: {result.Message}", "Info");
        }
        catch (Exception ex)
        {
            await _loggerService.LogAsync($"An exception occurred: {ex.Message}", "Error", ex.ToString());
        }
    }

    private async Task ProcessDeleteDevicesMessage(string jsonInput)
    {
        try
        {
            var deleteDevicesMessage = JsonConvert.DeserializeObject<DeleteDevicesMessage>(jsonInput);
            if (deleteDevicesMessage == null)
            {
                await _loggerService.LogAsync("Failed to deserialize JSON input for DeleteDevices.", "Error");
                throw new InvalidOperationException("Failed to deserialize the JSON input into a DeleteDevicesMessage.");
            }
            await _loggerService.LogAsync("Processing DeleteDevices operation", "Info");
            var result = await _deviceService.DeleteDevices(deleteDevicesMessage);
            await _loggerService.LogAsync($"Operation result: {result.Message}", "Info");
        }
        catch (Exception ex)
        {
            await _loggerService.LogAsync($"An exception occurred: {ex.Message}", "Error", ex.ToString());
        }
    }
}