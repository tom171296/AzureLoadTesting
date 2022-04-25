using Microsoft.Azure.Cosmos;

namespace src
{
    public class CosmosAgent
    {
        private readonly CosmosClient _client;
        private Database _database;
        private Container _container;

        public CosmosAgent(CosmosClient client)
        {
            _client = client;
        }

        public async Task SetupDatabase()
        {
            _database = await _client.CreateDatabaseIfNotExistsAsync("ToDoList");
            _container = await _database.CreateContainerIfNotExistsAsync("Items", "/partitionKey");
        }

        public async Task CreateWeatherForecast()
        {
            if (_container == null)
            {
                await SetupDatabase();
            }

            var weatherForecast = new WeatherForecast
            {
                Id = Guid.NewGuid(),
                PartitionKey = "sunny",
                Date = DateTime.Now,
                TemperatureC = 15,
                Summary = "It's sunny!"
            };
            await _container.CreateItemAsync(weatherForecast, new PartitionKey(weatherForecast.PartitionKey));
        }

        public async Task<WeatherForecast[]> GetWeatherForecastAsync()
        {
            if (_container == null)
            {
                await SetupDatabase();
            }

            var query = _container?
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
