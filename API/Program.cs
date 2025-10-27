using Application.Services;
using Infrastructure.Handlers;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Serilog;
using API.Middlewares;
using Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------
//  CONFIGURATION DU LOGGING (Serilog)
// -----------------------------------------------------
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/app_log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// -----------------------------------------------------
//  CONFIGURATION DES SERVICES
// -----------------------------------------------------
builder.Services.AddControllers();

// Swagger pour test et doc API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injection des d�pendances
builder.Services.AddSingleton<IJsonFileHandler,JsonFileHandler>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerService,PlayerService>();

var app = builder.Build();

// -----------------------------------------------------
//  PIPELINE HTTP
// -----------------------------------------------------
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

// -----------------------------------------------------
//  D�MARRAGE DE L'APPLICATION
// -----------------------------------------------------
try
{
    Log.Information("Application Tennis API d�marr�e");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Erreur critique lors du d�marrage de l'application.");
}
finally
{
    Log.CloseAndFlush();
}
