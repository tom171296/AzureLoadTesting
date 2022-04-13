using Microsoft.Azure.Cosmos;

namespace src 
{
    public class CosmosAgent
    {
        private readonly CosmosClient _client;

        public CosmosAgent(CosmosClient client)
        {
            _client = client;
        }

        public async Task<WeatherForecast[]> GetWeatherForecastAsync()
        {
            var query = _client.GetContainer("<your-database-id>", "<your-container-id>")
                .GetItemQueryIterator<WeatherForecast>(new QueryDefinition("SELECT * FROM c"));

            var forecasts = new List<WeatherForecast>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                forecasts.AddRange(response);
            }

            return forecasts.ToArray();
        }

    }
}
