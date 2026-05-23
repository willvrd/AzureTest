// -----------------------------------------------------------------------------
// Author:      William Verde
// Date:        2026
// License:     MIT
// Repository:  https://github.com/willvrd/AzureTest
// -----------------------------------------------------------------------------


using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Modules.Core.Middlewares.Handlers;
using WebApplication1.Modules.Media.Services.Interfaces;
using WebApplication1.Modules.Posts.Services;
using WebApplication1.Modules.Posts.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//Check Enviroment
var env = builder.Environment.EnvironmentName;
Console.WriteLine($"*** ENVIROMENT: {env} ***");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// IMPORTANTE: Agregar ProblemDetails primero | Exception Global Handler
builder.Services.AddProblemDetails();

//Registrar Exceptions | Exception Global Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Obtener la cadena de conexión de appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar el DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        // Esto le dice a EF que si Azure está "dormido", reintente automáticamente
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

//Services
builder.Services.AddScoped<IStorageService, BlobStorageService>();
builder.Services.AddScoped<IPostService, PostService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.MapOpenApi();
}

//Exception Global Handler
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
