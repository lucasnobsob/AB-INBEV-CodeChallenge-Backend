using AB_INBEV.Infra.CrossCutting.IoC;
using AB_INBEV.Services.Api.Configurations;
using AB_INBEV.Services.Api.StartupExtensions;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
IConfiguration Configuration = builder.Configuration;
IWebHostEnvironment _env = builder.Environment;

// Add services to the container.

// ----- Database -----
builder.Services.AddCustomizedDatabase(Configuration, _env);

// ----- Auth -----
builder.Services.AddCustomizedAuth(Configuration);

// ----- Http -----
builder.Services.AddCustomizedHttp(Configuration);

// ----- AutoMapper -----
builder.Services.AddAutoMapperSetup();

// Adding MediatR for Domain Events and Notifications
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

builder.Services.AddCustomizedHash(Configuration);

// ----- Swagger UI -----
builder.Services.AddCustomizedSwagger(_env);

// ----- Health check -----
builder.Services.AddCustomizedHealthCheck(Configuration, _env);

// .NET Native DI Abstraction
NativeInjectorBootStrapper.RegisterServices(builder.Services);

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

var app = builder.Build();

// ----- Error Handling -----
app.UseCustomizedErrorHandling(_env);

app.UseRouting();

// ----- CORS -----
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// ----- Auth -----
app.UseCustomizedAuth();

app.MapControllers();

//app.MapHealthChecks("/health");

//HealthCheckExtension.UseCustomizedHealthCheck(app, _env);

// ----- Swagger UI -----
app.UseCustomizedSwagger(_env);

app.Run();
