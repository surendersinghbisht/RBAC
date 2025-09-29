# Stage 1: Build the project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file first (helps with caching dependencies)
COPY ["RBAC.sln", "."]
COPY . .

# Restore NuGet packages
RUN dotnet restore

# Build and publish the project
RUN dotnet publish -c Release -o /app --no-restore

# Stage 2: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app .

# Expose port 80 (default for ASP.NET Core)
EXPOSE 80

# Set the entry point to run the app
ENTRYPOINT ["dotnet", "RBAC.dll"]
