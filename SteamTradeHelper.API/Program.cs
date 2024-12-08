using Microsoft.EntityFrameworkCore;
using SteamTradeHelper.Context;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Services.Infrastructure;
using SteamTradeHelper.Repositories.Infrastructure;
using SteamTradeHelper.Client.Infrastructure;
using SteamTradeHelper.Mappings.Infrastructure;
using SteamTradeHelper.API.Infrastructure.Cors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton(new HttpClient());
builder.Services.AddDbContext<DataContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("SteamTradeHelper")));
builder.Services.Configure<SteamSettings>(builder.Configuration.GetSection("SteamSettings"));

builder.Services.AddCustomRepositories();
builder.Services.AddCustomServices();
builder.Services.AddCustomClient();
builder.Services.AddCustomAutoMapper();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
