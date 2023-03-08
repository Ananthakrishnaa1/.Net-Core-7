using Main.Service.Models;
using Main.Service.RabbitMQ;
using Main.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Main.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherService _weatherService;
    private readonly IRabitMQProducer _rabitMQProducer;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService, IRabitMQProducer rabitMQProducer)
    {
        _logger = logger;
        _weatherService = weatherService;
        _rabitMQProducer = rabitMQProducer;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public Task<IEnumerable<WeatherForecast>> Get()
    {
        var weatherForcastList = _weatherService.GetWeatherAsync();
        _rabitMQProducer.SendWeatherMessage(weatherForcastList.Result);
        return weatherForcastList;
    }

    [HttpGet("{location}", Name= "GetWeatherForecastForLocation")]
    public Task<WeatherForecast> GetWeatherForecastForLocation(string location)
    {
        var weatherForcastList = _weatherService.GetWeatherOnLocation(location);
        _rabitMQProducer.SendWeatherMessage(weatherForcastList.Result);
        return weatherForcastList;
    }
}
