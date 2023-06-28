# Work-Tracking-Back-End

This is a sample project built with C# ASP.NET Core 6.0 and MySql.Data.MySqlClient.

## Prerequisites

Before running this project, make sure you have the following prerequisites installed:

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)

## Getting Started

To get started with this project, follow the steps below:

1. Clone the repository:
git clone https://github.com/LeoBonki/Work-Tracking-Back-End.git


2. Navigate to the project directory:
cd project-name

3. Install the required NuGet packages:
dotnet restore


4. Update the database connection string in the `appsettings.json` file:
"ConnectionStrings": {
"DefaultConnection": "Server=localhost;Database=your-database;Uid=your-username;Pwd=your-password;"
}


5. Run the database migrations:
dotnet ef database update


6. Start the application:
dotnet run


7. Open your web browser and navigate to `http://localhost:7036` to see the application.

## Libraries Used

This project utilizes the following libraries:

- [MySql.Data.MySqlClient](https://www.nuget.org/packages/MySql.Data/)
