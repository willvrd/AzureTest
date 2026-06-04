// -----------------------------------------------------------------------------
// Author:      William Verde
// Date:        2026
// License:     MIT
// Repository:  https://github.com/willvrd/AzureTest
// -----------------------------------------------------------------------------
using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Modules.Blog.Posts.Services;
using WebApplication1.Modules.Blog.Posts.Services.Interfaces;
using WebApplication1.Modules.Core.Middlewares.Handlers;
using WebApplication1.Modules.Media.Services.Interfaces;
using WebApplication1.Modules.Users.Entities;
using WebApplication1.Modules.Users.Seeders;

var builder = WebApplication.CreateBuilder(args);

//=============================================================================
// CONFIGURACIÓN DEL ENTORNO
//=============================================================================
var env = builder.Environment.EnvironmentName;
Console.WriteLine($"\n*** ENVIRONMENT: {env} ***\n");

//=============================================================================
// SERVICIOS BASE DEL CONTENEDOR
//=============================================================================
builder.Services.AddControllers();
builder.Services.AddOpenApi();

//=============================================================================
// CONFIGURACIÓN DE CORS (Conexión con Frontend)
//=============================================================================
/*
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAstroFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4321",                               // Astro Local
                "https://xxxxx.azurestaticapps.net"  // Tu Azure SWA
               )
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
*/

//=============================================================================
// API VERSIONING
//=============================================================================
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

//=============================================================================
// MANEJO GLOBAL DE EXCEPCIONES
//=============================================================================
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//=============================================================================
// PERSISTENCIA DE DATOS (DbContext)
//=============================================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

//=============================================================================
// CONFIGURACIÓN DE IDENTITY
//=============================================================================
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

//=============================================================================
// INYECCIÓN DE DEPENDENCIAS - SERVICES
//=============================================================================
builder.Services.AddScoped<IStorageService, BlobStorageService>();
builder.Services.AddScoped<IPostService, PostService>();

//=============================================================================
// BUILD APP
//=============================================================================
var app = builder.Build();

//=============================================================================
// PIPELINE DE PETICIONES HTTP (Middlewares)
//=============================================================================
// Manejo de errores al principio del ciclo
app.UseExceptionHandler();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// CORS debe ejecutarse SIEMPRE antes de Authorization y el mapeo de rutas
//app.UseCors("AllowAstroFrontend");

//First Authentication to tokens later
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


//=============================================================================
// SEEDERS
//=============================================================================
// USER MODULE
var runUserModuleSeeders = app.Configuration.GetValue<bool>("UserSettings:RunSeeders");
if (runUserModuleSeeders)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[SEEDER] 🚀 UserModule - RoleSeeder started...");
            Console.ResetColor();

            var roleManager = services.GetRequiredService<RoleManager<Role>>();
            await RoleSeeder.SeedRolesAsync(roleManager);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[SEEDER] ✅ UserModule - RoleSeeder completed successfully.\n");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[SEEDER] ❌ Error in UserModule - RoleSeeder: {ex.Message}\n");
            Console.ResetColor();

            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the User module roles.");
        }
    }
}

//FINAL - APP RUN
app.Run();