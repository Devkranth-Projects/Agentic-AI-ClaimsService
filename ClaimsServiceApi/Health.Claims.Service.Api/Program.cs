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
//builder.WebHost.UseUrls("http://+:9090"); // Bind to all interfaces in container

if (builder.Environment.IsEnvironment("Docker"))
{
    builder.WebHost.UseUrls("http://+:9090"); // Only bind 9090 in Docker
}

// ------------------------------
// 2️⃣ Database Configuration
// ------------------------------
var databaseProvider = builder.Configuration
    .GetValue<string>("DatabaseProvider")
    ?? throw new InvalidOperationException("DatabaseProvider configuration is missing.");

var connectionString = builder.Configuration
    .GetConnectionString(databaseProvider)
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
builder.Services.AddAutoMapper(typeof(ClaimantProfile).Assembly);
builder.Services.AddScoped<IClaimService, ClaimService>();

// ------------------------------
// 4️⃣ Application Layer
// ------------------------------
builder.Services.AddAutoMapper(typeof(ClaimService).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<SubmitClaimRequestValidator>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IClaimantService, ClaimantService>();
builder.Services.AddScoped<IPolicyService, PolicyService>();

// ------------------------------
// 5️⃣ Web Services
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
// 6️⃣ CORS Configuration
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
// 7️⃣ Build app
// ------------------------------
var app = builder.Build();

// ------------------------------
// 8️⃣ Middleware pipeline
// ------------------------------
app.UseCors("CorsPolicy");
app.UseMiddleware<ExceptionHandlingMiddleware>();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Apply EF Core migrations automatically in dev
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClaimsDataServiceDBContext>();
    db.Database.Migrate();
//}

// Only use HTTPS redirection if NOT running in Docker
if (!builder.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
