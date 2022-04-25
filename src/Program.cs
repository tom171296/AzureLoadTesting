using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using src;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(x => new CosmosClient(
    "https://azureloadtesttvdb.documents.azure.com:443/", 
    "oD2YdsiOKTlpfPL3Vx8bjDqak7R834B8uV8IjhwXhO8RRUJOIZSlLpV8Oijf6qEAAJNePgC9c9tRtoruE9pyTQ=="
    ));
builder.Services.AddTransient<CosmosAgent>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("setupdatabase", async (CosmosAgent cosmosAgent) =>
{
    await cosmosAgent.SetupDatabase();
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