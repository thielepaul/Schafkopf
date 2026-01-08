# Developer Setup Guide for AI Agents

This guide provides step-by-step instructions for AI agents and automated systems to set up the Schafkopf development environment.

## Prerequisites

- .NET SDK 6.0 or higher installed
- Node.js/npm (for frontend dependencies)
- Git

## Initial Setup

### 1. Clone the Repository

```bash
git clone https://github.com/mkre/Schafkopf
cd Schafkopf
```

### 2. Generate HTTPS Developer Certificate

**IMPORTANT**: The application requires an HTTPS developer certificate to run. This is a common requirement for ASP.NET Core applications in development.

Generate the certificate:

```bash
dotnet dev-certs https
```

This creates a self-signed certificate in your local certificate store. You only need to run this once per development machine.

### 3. Restore NuGet Packages

```bash
dotnet restore
```

### 4. Build the Project

```bash
dotnet build
```

### 5. Run the Application

```bash
cd Schafkopf
dotnet run
```

The application will start listening on:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001

**Dev Container Note**: The VS Code dev container is configured with port forwarding (`devcontainer.json`), which automatically maps container ports 5000 and 5001 to your host machine. VS Code will show a prompt to open the application when it detects the ports are in use.

## Troubleshooting

### HTTPS Certificate Error

If you encounter an error like:
```
Unable to configure HTTPS endpoint. No server certificate was specified, and the default developer certificate could not be found or is out of date.
```

**Solution**: Run the certificate generation command again:
```bash
dotnet dev-certs https
```

### Port Already in Use

If ports 5000 or 5001 are already in use, you can specify custom ports:

```bash
dotnet run --urls "http://localhost:5002;https://localhost:5003"
```

## Watch Mode Development

To run the application in watch mode (automatically restarts on code changes):

```bash
dotnet watch run
```

## Running Tests

```bash
dotnet test
```

## Building for Production

```bash
dotnet publish -c Release
```

## Common Tasks

### View Project Structure

The project is organized as follows:
- `Schafkopf/` - Main ASP.NET Core application
- `Schafkopf.Tests/` - Unit tests
- `Schafkopf/Controllers/` - MVC controllers
- `Schafkopf/Hubs/` - SignalR hubs for real-time communication
- `Schafkopf/GameState/` - Game logic and state management
- `Schafkopf/Models/` - Data models
- `Schafkopf/Views/` - Razor views
- `Schafkopf/wwwroot/` - Static files (CSS, JavaScript, images)

### Debugging

Set breakpoints in Visual Studio Code and use the built-in debugger, or attach to a running process:

```bash
dotnet run
```

Then attach your debugger to the process.

## Notes for Agents

- Always run `dotnet dev-certs https` when setting up a new development environment
- The certificate is machine-specific; it's stored in your local certificate store and not committed to git
- Build tasks are configured in `.vscode/tasks.json` if using Visual Studio Code
- The solution uses .NET 6.0 as the target framework

## Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [SignalR Documentation](https://docs.microsoft.com/aspnet/core/signalr)
- [.NET CLI Documentation](https://docs.microsoft.com/dotnet/core/tools)
