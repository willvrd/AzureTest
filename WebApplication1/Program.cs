// -----------------------------------------------------------------------------
// Author:      William Verde
// Date:        2026
// License:     MIT
// Repository:  https://github.com/willvrd/AzureTest
// -----------------------------------------------------------------------------


using Asp.Versioning; // Added for API Versioning
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Modules.Core.Middlewares.Handlers;
using WebApplication1.Modules.Media.Services.Interfaces;
using WebApplication1.Modules.Posts.Services;
using WebApplication1.Modules.Posts.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Check Environment
var env = builder.Environment.EnvironmentName;
Console.WriteLine($"*** ENVIRONMENT: {env} ***");

// Add services to the container.
builder.Services.AddControllers();

// Configure API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true; // Uses v1 if no version is provided in URL
    options.ReportApiVersions = true;                  // Adds version info to headers (api-supported-versions)
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";                // Formats the version group as 'v1', 'v1.1', etc.
    options.SubstituteApiVersionInUrl = true;          // Automatically maps the route parameter
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// IMPORTANTE: Agregar ProblemDetails primero | Exception Global Handler
builder.Services.AddProblemDetails();

// Registrar Exceptions | Exception Global Handler
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

// Services
builder.Services.AddScoped<IStorageService, BlobStorageService>();
builder.Services.AddScoped<IPostService, PostService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.MapOpenApi();
}

// Exception Global Handler
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
