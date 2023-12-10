using System.Text;
using EventPlanning.Interfaces;
using EventPlanning.Database.Repositories;
using EventPlanning.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EventPlanning.Database;
using EventPlanning.Models;
using MongoDB.Driver;
using EventPlanning.Services.Messages;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddSingleton<AppDbContext>();
builder.Services.AddScoped<IMongoCollection<User>>(serviceProvider =>
{
    var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
    return dbContext.Users; 
});

builder.Services.AddScoped<IMongoCollection<Event>>(serviceProvider =>
{
    var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
    return dbContext.Events;
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();



builder.Services.AddSingleton<MessageServiceFactory>();
builder.Services.AddTransient<EmailMessageService>();
builder.Services.AddTransient<SmsMessageService>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        });

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

