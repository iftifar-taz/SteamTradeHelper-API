using Microsoft.EntityFrameworkCore;
using SteamTradeHelper.API.Infrastructure.Cors;
using SteamTradeHelper.API.Infrastructure.Middlewares;
using SteamTradeHelper.Client.Infrastructure;
using SteamTradeHelper.Context;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Mappings.Infrastructure;
using SteamTradeHelper.Repositories;
using SteamTradeHelper.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton(new HttpClient());
builder.Services.AddDbContext<DataContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("SteamTradeHelper")));
builder.Services.Configure<SteamSettings>(builder.Configuration.GetSection("SteamSettings"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(
        AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly => assembly.FullName != null && assembly.FullName.StartsWith("SteamTradeHelper.Services"))
            .ToArray()
    );
});
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
app.UseCustomMiddlewares();

app.MapControllers();

app.Run();
