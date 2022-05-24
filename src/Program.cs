using Azure.Identity;
using Microsoft.Azure.Cosmos;
using src;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(x => new CosmosClient(
    ""
    ));
builder.Services.AddTransient<CosmosAgent>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();

app.MapGet("setupdatabase", async (CosmosAgent cosmosAgent, ILogger<Program> logger) =>
{
    try{
        await cosmosAgent.SetupDatabase();
    }
    catch(Exception e){
        logger.LogError("Something went wrong {message}", e.Message);
        throw;
    }
});

app.MapPut("createweatherforecast", async (CosmosAgent cosmosAgent) =>
{
    await cosmosAgent.CreateWeatherForecast();
});

app.MapGet("/weatherforecast", async (CosmosAgent cosmosAgent) =>
{
    return await cosmosAgent.GetWeatherForecastAsync();
})
.WithName("GetWeatherForecast");

app.Run();