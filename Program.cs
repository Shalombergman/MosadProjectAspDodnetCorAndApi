using Microsoft.EntityFrameworkCore;
using MosadApiServer.Controllers;
using MosadApiServer.Data;
using MosadApiServer.Enums;
using MosadApiServer.Interfaces;
using MosadApiServer.Models;
using MosadApiServer.Servises;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddScoped<ServiceMission>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<AgentsController>();
builder.Services.AddScoped<MissionsController>();
builder.Services.AddScoped<ServiceMatrix>();
//builder.Services.AddScoped<ServiceMission>();
//builder.Services.AddScoped<ServiceMoving>();

builder.Services.AddScoped<IServiceMoving, ServiceMoving>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
