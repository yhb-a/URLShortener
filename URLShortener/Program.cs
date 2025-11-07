using Microsoft.EntityFrameworkCore;
using URLShortener;
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
builder.Services.AddScoped<IURLRepository, URLRepository>();

// Services
builder.Services.AddScoped<IURLService, URLService>();
builder.Services.AddSingleton<IGlobalCounter, GlobalCounter>();

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
