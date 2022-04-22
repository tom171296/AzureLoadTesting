using Microsoft.Azure.Cosmos;
using src;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(x => new CosmosClient(""));
builder.Services.AddTransient<CosmosAgent>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();

app.MapGet("/weatherforecast", async (CosmosAgent cosmosAgent) =>
{
    return await cosmosAgent.GetWeatherForecastAsync();
})
.WithName("GetWeatherForecast");

app.Run();

public record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}