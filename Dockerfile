# Stage 1: Build the project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["RBAC.sln", "."]
COPY . .

# Restore dependencies
RUN dotnet restore

# Build and publish
RUN dotnet publish -c Release -o /app

# Stage 2: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app .

# Expose port (adjust if your app uses a different port)
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "RBAC.dll"]
