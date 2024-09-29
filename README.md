## Honeywell Web Video Player

This is a web video player that can play videos from a URL. It is built using ASP.NET Core 6 MVC. The application is to
provide the following features for its users:

- Allow users to see a list of video files that have been uploaded to a server media folder
- Allow users to upload new MP4 video files into the server media folder
- Allow users to playback any MP4 video file that is in the server media folder

## Prerequisites

- Visual Studio 2022
- .NET 6 SDK
- ASP.NET Core 6
- Entity Framework Core 6
- Docker Desktop

## Tech stack:

- Bootstrap 5
- Font Awesome 5
- knockout.js
- ASP.NET MVC
- Unit Testing
- NSubstitute
- FluentAssertions
- MSSQL Server (Docker)

## Code Coverage Report:
- Navigate to folder "coverage-report" and open the "index.html" file in the browser to view the code coverage report.

## Getting Started

1. Clone the repository:
   ```bash
   git clone
   ```
2. Navigate to the root directory of the project.
3. Run the following command to build and containerize the MSSQL Server in a Docker image:
   ```bash
   docker compose -f docker-compose.yml up -d
   ```
4. Open the solution in **Visual Studio 2022+**.
5. Build the solution:
   ```bash
   dotnet build
   ```
6. Run the unit tests to verify the application:
   ```bash
   dotnet test
   ```
7. Run the application:
   ```bash
   dotnet run -p src/Application/HoneywellWeb/HoneywellWeb.csproj
   ```
8. Navigate to the following URL to view the application:
    - **https://localhost:5002**
    - **http://localhost:5001**
