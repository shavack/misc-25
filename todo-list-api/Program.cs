using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoListApi.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TodoListApi.Application.Mapping;
using Microsoft.Extensions.Configuration;
using TodoListApi.Application.Services;
using FluentValidation;
using TodoListApi.Application.Dtos;
using TodoListApi.Application.Validators;

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
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add application services
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IValidator<TaskItemDto>, TaskItemValidator>();

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapTaskEndpoints();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler("/error");

app.Logger.LogInformation("The app started");

app.Run();