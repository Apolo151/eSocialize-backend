# Use the .NET 8 SDK to build and run the app in development mode
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install any required dependencies (optional)
# RUN apt-get update && apt-get install -y some-dependency

# Copy everything and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the app
COPY . ./

# Use the Development environment
ENV ASPNETCORE_ENVIRONMENT=Development

# Enable Hot Reload
ENV DOTNET_USE_POLLING_FILE_WATCHER=true

# Command to run the app in development mode with hot reload
CMD ["dotnet", "watch", "run", "--no-launch-profile", "--urls", "http://0.0.0.0:8080"]