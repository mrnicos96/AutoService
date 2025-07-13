using AutoService.API.Extensions;
using AutoService.API.Middleware;
using AutoService.Application;
using AutoService.Infrastructure;
using OfficeOpenXml;



var builder = WebApplication.CreateBuilder(args);



// Add services to the container
builder.Services
    .AddApplication()      // ���� Application
    .AddInfrastructure(builder.Configuration); // ���� Infrastructure

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ��������� CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();

// ��������� ��������� middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.MapControllers();

app.Run();