using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Health.Claims.Service.Infrastructure.Data.Context
{
    public class ClaimsDataServiceDbContextFactory
        : IDesignTimeDbContextFactory<ClaimsDataServiceDBContext>
    {
        public ClaimsDataServiceDBContext CreateDbContext(string[] args)
        {
            // Detect environment (default to Development)
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Base path for appsettings
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables() // Allow overriding via environment vars
                .Build();

            // Read database provider and connection string
            var provider = configuration.GetValue<string>("DatabaseProvider");
            if (string.IsNullOrWhiteSpace(provider))
                throw new InvalidOperationException("DatabaseProvider is not configured in appsettings.");

            var connectionString = configuration.GetConnectionString(provider);
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException($"Connection string for provider '{provider}' not found.");

            // Build DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<ClaimsDataServiceDBContext>();

            switch (provider.ToLowerInvariant())
            {
                case "postgresql":
                    optionsBuilder.UseNpgsql(connectionString);
                    break;
                case "sqlserver":
                    optionsBuilder.UseSqlServer(connectionString);
                    break;
                case "oracle":
                    optionsBuilder.UseOracle(connectionString);
                    break;
                case "mysql":
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported database provider: {provider}");
            }

            return new ClaimsDataServiceDBContext(optionsBuilder.Options);
        }
    }
}
