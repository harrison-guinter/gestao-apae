# Use the official .NET runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# Configurar timezone correto (Brasil - UTC-3)
ENV TZ=America/Sao_Paulo
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

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

# Criar usuário não-root
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Garantir timezone dentro da aplicação .NET
ENV TZ=America/Sao_Paulo

ENTRYPOINT ["dotnet", "SistemaApae.Api.dll"]
