# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

# Copy and restore project files
COPY Application/Application.csproj Application/
COPY Persistence/Persistence.csproj Persistence/
COPY Contracts/Contracts.csproj Contracts/
COPY Api/Api.csproj Api/

RUN dotnet restore Application/Application.csproj
RUN dotnet restore Persistence/Persistence.csproj
RUN dotnet restore Contracts/Contracts.csproj
RUN dotnet restore Api/Api.csproj

# Copy the remaining source code
COPY . ./

# Build the solution
RUN dotnet build ./Api/Api.csproj -c Release --no-restore

# Publish the projects
RUN dotnet publish ./Api/Api.csproj -c Release -o /app/publish --no-restore --no-build

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final

WORKDIR /app

# Copy the published projects from the build image
COPY --from=build /app/publish .

# Start the application
ENTRYPOINT ["dotnet", "Lilibre.Api.dll"]
