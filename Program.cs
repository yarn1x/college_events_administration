using college_events_admin_API.Models;
using college_events_admin_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // указывает, будет ли валидироваться издатель при валидации токена
            
            ValidIssuer = AuthOptions.ISSUER,// строка, представляющая издателя
            
            ValidateAudience = true,// будет ли валидироваться потребитель токена
            
            ValidAudience = AuthOptions.AUDIENCE,// установка потребителя токена
            
            ValidateLifetime = true,// будет ли валидироваться время существования
            
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),// установка ключа безопасности
            
            ValidateIssuerSigningKey = true,// валидация ключа безопасности
        };
    });


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<EventsService>();
builder.Services.AddScoped<AuthorizationService>();
builder.Services.AddHostedService<BackgroundUpdateService>();
builder.Services.AddDbContext<SutrEventsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();