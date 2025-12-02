using AutoMapper;
using FluentValidation;
using Health.Claims.Service.Api.Middleware;
using Health.Claims.Service.Application.Interfaces;
using Health.Claims.Service.Application.Mappers;
using Health.Claims.Service.Application.Validators;
using Health.Claims.Service.Domain.Interfaces;
using Health.Claims.Service.Infrastructure;
using Health.Claims.Service.Infrastructure.Data.Context;
using Health.Claims.Service.Infrastructure.Messaging;
using Health.Claims.Service.Infrastructure.Repositories;
using Health.Claims.Service.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------
// 1️⃣ Configure Kestrel for Docker
// ------------------------------
if (builder.Environment.IsEnvironment("Docker"))
{
    builder.WebHost.UseUrls("http://+:9090");
}

// ------------------------------
// 2️⃣ Database Configuration
// ------------------------------
var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider")
    ?? throw new InvalidOperationException("DatabaseProvider configuration is missing.");

var connectionString = builder.Configuration.GetConnectionString(databaseProvider)
    ?? throw new InvalidOperationException($"Connection string for provider '{databaseProvider}' not found.");

builder.Services.AddDbContext<ClaimsDataServiceDBContext>(options =>
{
    switch (databaseProvider.ToLowerInvariant())
    {
        case "postgresql":
            options.UseNpgsql(connectionString);
            break;
        case "sqlserver":
            options.UseSqlServer(connectionString);
            break;
        case "oracle":
            options.UseOracle(connectionString);
            break;
        case "mysql":
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            break;
        default:
            throw new InvalidOperationException($"Unsupported database provider: {databaseProvider}");
    }
});

// ------------------------------
// 3️⃣ Infrastructure Layer
// ------------------------------
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IClaimantRepository, ClaimantRepository>();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IClaimStatusRepository, ClaimStatusRepository>();
builder.Services.AddSingleton<IMessageProducer, RabbitMqProducer>();

// AutoMapper (only once)
builder.Services.AddAutoMapper(typeof(ClaimantProfile).Assembly);

// Application services
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IClaimantService, ClaimantService>();
builder.Services.AddScoped<IPolicyService, PolicyService>();

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<SubmitClaimRequestValidator>();

// ------------------------------
// 4️⃣ Web Services
// ------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// ------------------------------
// 5️⃣ CORS
// ------------------------------
var allowedOrigins = builder.Configuration
    .GetValue<string>("AllowedOrigins")
    ?.Split(';') ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
        else
        {
            policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
        }
    });
});

// ------------------------------
// 6️⃣ Build App
// ------------------------------
var app = builder.Build();

// ------------------------------
// 7️⃣ Middleware Pipeline (Correct Order)
// ------------------------------
app.UseCors("CorsPolicy");

// Custom exception middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Automatically apply migrations in DEV
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClaimsDataServiceDBContext>();
    db.Database.Migrate();
}

// Only use HTTPS redirect outside Docker
if (!app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

// REQUIRED for routing controllers
app.UseRouting();

// Authentication/Authorization must come AFTER routing
app.UseAuthorization();

// Map API controllers
app.MapControllers();

app.Run();
