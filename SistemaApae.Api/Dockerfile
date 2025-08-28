# Use the official .NET runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SistemaApae.Api.csproj", "."]
RUN dotnet restore "./SistemaApae.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SistemaApae.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SistemaApae.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "SistemaApae.Api.dll"]
