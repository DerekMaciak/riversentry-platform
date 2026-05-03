using Microsoft.EntityFrameworkCore;
using RiverSentry.Api.Hubs;
using RiverSentry.Api.Services;
using RiverSentry.Application.Interfaces;
using RiverSentry.Infrastructure;
using RiverSentry.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure (EF Core, repositories, application services)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=(localdb)\\mssqllocaldb;Database=RiverSentry;Trusted_Connection=True;";
builder.Services.AddInfrastructure(connectionString);

// SignalR
builder.Services.AddSignalR();
builder.Services.AddScoped<IAlertBroadcaster, SignalRAlertBroadcaster>();

// API
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Health checks — used by Azure Always On and external monitors to keep the app warm
builder.Services.AddHealthChecks();

// CORS — allow web and mobile clients
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClients", policy =>
    {
        policy.WithOrigins(
                builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? ["http://localhost:5002", "http://localhost:5210"])
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RiverSentryDbContext>();
    await db.Database.MigrateAsync();
    await db.SeedDevicesAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowClients");
app.UseAuthorization();

app.MapControllers();
app.MapHub<AlertHub>("/hubs/alerts");
app.MapHealthChecks("/health");

app.Run();
