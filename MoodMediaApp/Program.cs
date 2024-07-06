using Microsoft.Extensions.DependencyInjection;
using MoodMediaApp.Data;
using MoodMediaApp.Services;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;
using MoodMediaApp.Interfaces;
using MoodMediaApp.Messages;
using FluentValidation;
using MoodMediaApp.Validators;

class Program
{
    static async Task Main()
    {
        try
        {
            var configuration = BuildConfiguration();
            var serviceProvider = ConfigureServices(configuration);

            using var scope = serviceProvider.CreateScope();
            var loggerService = scope.ServiceProvider.GetService<ILoggerService>() ?? throw new InvalidOperationException("Logger service is not configured.");
            var messageProcessor = scope.ServiceProvider.GetService<MessageProcessingService>() ?? throw new InvalidOperationException("Message Processing service is not configured.");

            await loggerService.LogAsync("Application started", "Info");

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Process message");
                Console.WriteLine("2. Exit");
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Please enter the JSON for the operation. Enter an empty line to finish:");
                        string jsonInput = ReadJsonInput();
                        if (!string.IsNullOrWhiteSpace(jsonInput))
                        {
                            try
                            {
                                await messageProcessor.ProcessMessageAsync(jsonInput);
                            }
                            catch (Exception ex)
                            {
                                await loggerService.LogAsync($"An exception occurred: {ex.Message}", "Error", ex.ToString());
                            }
                            
                        }
                        else
                        {
                            await loggerService.LogAsync("No JSON input was detected", "Warning");
                        }
                        break;

                    case "2":
                        exit = true;
                        await loggerService.LogAsync("Application exited", "Info");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }

    }

    private static string ReadJsonInput()
    {
        var input = new StringBuilder();
        string? line;
        while (!string.IsNullOrEmpty(line = Console.ReadLine()))
        {
            input.Append(line);
        }
        return input.ToString();
    }

    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    private static ServiceProvider ConfigureServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();
        services.AddScoped<IDatabaseOperation, DatabaseOperation>(provider => 
            new DatabaseOperation(configuration.GetConnectionString("DefaultConnection")));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        services.AddTransient<IValidator<NewCompanyMessage>, NewCompanyMessageValidator>();
        services.AddTransient<ICompanyService, CompanyService>();
        services.AddTransient<IValidator<DeleteDevicesMessage>, DeleteDevicesMessageValidator>();
        services.AddTransient<IDeviceService, DeviceService>();
        services.AddSingleton<ILoggerService, LoggerService>();
        services.AddSingleton<MessageProcessingService>();

        return services.BuildServiceProvider();
    }

}