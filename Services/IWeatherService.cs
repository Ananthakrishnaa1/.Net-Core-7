using Main.Service.Models;

namespace Main.Service.Services
{
    public interface IWeatherService
    {
        public Task<IEnumerable<WeatherForecast>> GetWeatherAsync();
        Task<WeatherForecast> GetWeatherOnLocation(string location);
    }
}
