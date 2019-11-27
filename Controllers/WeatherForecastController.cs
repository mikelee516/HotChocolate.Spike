using System.Collections.Generic;
using HotChocolate.Spike.Models;
using HotChocolate.Spike.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HotChocolate.Spike.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("Pass")]
    public class PassWeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastRepository _repository;
        private readonly ILogger<PassWeatherForecastController> _logger;

        public PassWeatherForecastController(WeatherForecastRepository repository, ILogger<PassWeatherForecastController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _repository.GetWeatherForecasts();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    [Authorize("Deny")]
    public class DenyWeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastRepository _repository;
        private readonly ILogger<DenyWeatherForecastController> _logger;

        public DenyWeatherForecastController(WeatherForecastRepository repository, ILogger<DenyWeatherForecastController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _repository.GetWeatherForecasts();
        }
    }
}
