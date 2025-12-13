using Microsoft.EntityFrameworkCore;
using URLShortener.Repository;
using URLShortener.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<URLDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUrlRepository, UrlRepository>();

// Services
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddSingleton<IGlobalCounterService, GlobalCounterService>();
builder.Services.AddHostedService<CounterInitializerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
