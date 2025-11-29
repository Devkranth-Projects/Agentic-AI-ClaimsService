# -----------------------
# Build Stage
# -----------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files first to leverage layer caching
COPY ClaimsServiceApi/Health.Claims.Service.Api/*.csproj ClaimsServiceApi/Health.Claims.Service.Api/
COPY ClaimsServiceApi/Health.Claims.Service.Application/*.csproj ClaimsServiceApi/Health.Claims.Service.Application/
COPY ClaimsServiceApi/Health.Claims.Service.Domain/*.csproj ClaimsServiceApi/Health.Claims.Service.Domain/
COPY ClaimsServiceApi/Health.Claims.Service.Infrastructure/*.csproj ClaimsServiceApi/Health.Claims.Service.Infrastructure/

# Restore NuGet packages for API project (and it will pick up dependencies)
RUN dotnet restore ClaimsServiceApi/Health.Claims.Service.Api/Health.Claims.Service.Api.csproj

# Copy all source files
COPY . .

# Set working directory to API project
WORKDIR /src/ClaimsServiceApi/Health.Claims.Service.Api

# Publish the API (use Release for production)
RUN dotnet publish Health.Claims.Service.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# -----------------------
# Runtime Stage
# -----------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Configure runtime
#ENV ASPNETCORE_URLS=http://+:9090
#EXPOSE 9090

ENTRYPOINT ["dotnet", "Health.Claims.Service.Api.dll"]
