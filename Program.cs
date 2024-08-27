using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MosadApiServer.Controllers;
using MosadApiServer.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.IdentityModel.Tokens;
using System.Text; 
using MosadApiServer.Enums;
using MosadApiServer.Interfaces;
using MosadApiServer.Models;
using MosadApiServer.Servises;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
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

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SimulationPolicy", policy => 
        policy.RequireClaim("ServerName", "SimulationServer"));
    options.AddPolicy("MVCPolicy", policy => 
        policy.RequireClaim("ServerName", "MVCServer"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
