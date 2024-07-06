using Microsoft.Extensions.DependencyInjection;
using MoodMediaApp.Data;
using MoodMediaApp.Services;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;
using MoodMediaApp.Interfaces;

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
            Console.WriteLine("Please enter the JSON for the operation. Enter an empty line to finish:");

            string jsonInput = ReadJsonInput();

            if (!string.IsNullOrWhiteSpace(jsonInput))
            {
                try
                {
                    await messageProcessor.ProcessMessageAsync(jsonInput);
                    await loggerService.LogAsync($"Message processed successfully", "Info");
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
        services.AddTransient<ICompanyService, CompanyService>();
        services.AddSingleton<ILoggerService, LoggerService>();
        services.AddSingleton<MessageProcessingService>();

        return services.BuildServiceProvider();
    }

}