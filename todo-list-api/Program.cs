using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TodoListApi.Application.Mapping;
using TodoListApi.Application.Services;
using FluentValidation;
using TodoListApi.Application.Dtos;
using TodoListApi.Application.Validators;
using TodoListApi.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDbContext<AppDbContext>(options =>
  //  options.UseInMemoryDatabase("AppDbContext"));
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();
// Add AutoMapper
builder.Services.AddAutoMapper(typeof(TaskMappingProfile).Assembly);
// Add db context
builder.Services.AddPersistence(builder.Configuration);
//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add application services
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IValidator<TaskItemDto>, TaskItemValidator>();
builder.Services.AddScoped<IValidator<TaskQueryParams>, TaskQueryParamsValidator>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
app.UseCors();
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
app.MapTaskEndpoints();
//app.AddErrorHandling();

app.UseAuthorization();

app.MapControllers();

app.Logger.LogInformation("The app started");

app.Run();
public partial class Program { }; 
