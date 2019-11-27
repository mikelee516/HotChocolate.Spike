using System.Linq;
using HotChocolate.Spike.Models;
using HotChocolate.Spike.Repositories;
using Microsoft.Extensions.Logging;

namespace HotChocolate.Spike.GraphQL
{
    public class MyQuery
    {
        private readonly WeatherForecastRepository _weatherForecastRepository;
        private readonly ILogger<MyQuery> _logger;

        public MyQuery(
            WeatherForecastRepository weatherForecastRepository,
            ILogger<MyQuery> logger)
        {
            _weatherForecastRepository = weatherForecastRepository;
            _logger = logger;
        }

        public IQueryable<WeatherForecast> GetWeatherForecasts()
        {
            return _weatherForecastRepository.GetWeatherForecasts();
        }
    }
}