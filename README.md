
# Mood Media Coding Assignment

## Introduction
This project submission is part of the Mood Media coding assignment. It's a .NET console application designed to process JSON messages for creating companies and deleting devices, handling raw SQL queries, and utilizing MediatR for business logic separation.

## Project Structure
The application is structured as follows:
- **Data Access Layer (`Data`)**: Handles all database operations using Dapper to ensure efficient query processing and MediatR to manage command and query requests.
- **Commands (`Data/Commands`)**: Contains all command handlers and request models, facilitating the creation and deletion operations for companies and devices.
- **Queries (`Data/Queries`)**: Comprises handlers that verify the existence of records in the database, ensuring that duplicate records are not created.
- **Enums (`Enums`)**: Defines enums like `DeviceType` and `LicensingType` to standardize the types used across the application.
- **Interfaces (`Interfaces`)**: Defines the contracts for services and operations, ensuring a clear modular architecture and facilitating mock implementations for testing.
- **Messages (`Messages`)**: Defines the data transfer objects (DTOs) used for communicating between different parts of the application.
- **Services (`Services`)**: Implements the core business logic and integrates with the MediatR handlers, providing a separation of concerns and cleaner codebase.
- **Utilities (`Utilities`)**: Includes tools such as `SerialNumberUtility` for generating unique serial numbers.
- **Validators (`Validators`)**: Utilizes FluentValidation to ensure that incoming requests meet the specified criteria before processing.
- **Program.cs**: The entry point of the application which sets up dependency injection and starts the message processing service.

## Technologies Used
- **.NET Core**: Framework for building the console application.
- **MediatR**: Used for implementing the Mediator pattern, separating how requests are sent from how they are handled.
- **Dapper**: ORM used for data access, simplifying complex SQL transactions with straightforward object mapping.
- **FluentValidation**: Provides a way to validate objects based on rules, integrated into the MediatR pipeline.
- **SQL Server**: Database system used for storing all application data.
- **Dependency Injection (DI)**: Utilizes .NET Core's built-in DI framework to manage service lifecycles and dependencies throughout the application.

## Database Schema

This project includes a SQL Server database with the following tables, which support the application's data storage needs:

### Tables

1. **ApplicationLogs** - Stores log entries for application events.
   - LogID (INT, primary key, auto-incremented) - Unique identifier for log entries.
   - Timestamp (DATETIME) - The date and time the log was recorded.
   - LogLevel (VARCHAR(50)) - The severity level of the log entry.
   - Message (TEXT) - The log message content.
   - Exception (TEXT, nullable) - Details of any exceptions thrown.

2. **Company** - Contains company details.
   - Id (INT, primary key, auto-incremented) - Unique identifier for companies.
   - Name (NVARCHAR(255)) - The name of the company.
   - Code (NVARCHAR(50), unique) - A unique code assigned to the company.
   - Licensing (INT) - Licensing level/type for the company.

3. **Device** - Manages devices within companies.
   - Id (INT, primary key, auto-incremented) - Unique identifier for devices.
   - SerialNumber (NVARCHAR(255), unique) - Unique serial number for each device.
   - Type (INT) - The type of device.
   - LocationId (INT) - Reference to Location, associates device to a location.

4. **Location** - Details locations within companies.
   - Id (INT, primary key, auto-incremented) - Unique identifier for locations.
   - Name (NVARCHAR(255)) - Name of the location.
   - Address (NVARCHAR(MAX)) - Address of the location.
   - ParentId (INT) - Reference to Company, associates location to a company.

### Relationships

- Each **Device** is associated with one **Location**.
- Each **Location** is associated with a **Company**, and each company can have multiple locations.

This schema facilitates the management of company devices and logging of application activities efficiently.

## Getting Started

### Prerequisites
- .NET (6.0.0 or later)
- SQL Server (LocalDB or a full instance for production)
- Visual Studio 2022 or a compatible editor with C# support

### Setup and Installation
1. **Clone the repository**:
   ```
   git clone [repository-url]
   cd MoodMediaApp
   ```
2. **Database Setup**:
   -  Open the SQL Server Database Project within Visual Studio, located within the solution.
   -  Ensure SQL Server is running and properly configured on your local machine or network.
   - Right-click on the project in the Solution Explorer and select **Publish**.
   -  In the Publish Database dialog, specify the target database server and database name, and then click **Publish** to deploy the schema and any necessary objects directly to SQL Server.


3. **Application Configuration**:
   - Update `appsettings.json` to include your SQL Server connection string.

4. **Running the Application**:
   ```
   dotnet restore   # Restore NuGet packages
   dotnet build     # Build the project
   dotnet run       # Run the application
   ```

### How to Use
- The application will prompt you to input JSON strings based on the provided samples (`createCompany.json` and `deleteDevices.json`).
- Follow the prompts in the console to input the correct JSON format or terminate the input session.

## Testing
- The project includes unit tests for the service layer and integration tests for the data access layer, demonstrating the expected functionality and handling of edge cases.
