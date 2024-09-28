## Honeywell Web Video Player
This is a web video player that can play videos from a URL. It is built using ASP.NET Core 6 MVC. The application is to provide the following features for its users:
- Allow users to see a list of video files that have been uploaded to a server media folder
- Allow users to upload new MP4 video files into the server media folder
- Allow users to playback any MP4 video file that is in the server media folder

## Prerequisites
- Visual Studio 2022
- .NET 6 SDK
- Bootstrap 5
- Font Awesome 5
- ASP.NET Core 6
- Entity Framework Core 6
- Docker Desktop

## Getting Started
1. Clone the repository
```bash
git clone   
```
2. Open the solution in Visual Studio 2022
3. Build the solution
4. Run the application
5. Navigate to the URL: https://localhost:5002 || http://localhost:5001 to view the application

## Running the Application in Docker
1. Navigate to the root directory of the project
2. Run the following command to build the Docker image
```bash
docker compose -f docker-compose.yml up -d
```
3. Navigate to the URL: https://localhost:5002 || http://localhost:5001 to view the application
