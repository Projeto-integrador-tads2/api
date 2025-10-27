# Use the official .NET 8.0 runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET 8.0 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["Crm.csproj", "."]
RUN dotnet restore "Crm.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build "Crm.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Crm.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app

# Copy the published application from the publish stage
COPY --from=publish /app/publish .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Crm.dll"]
