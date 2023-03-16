using Main.Service.Models;
using Main.Service.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Main.Service.Services
{
    public class WeatherService : IWeatherService
    {
        private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private static readonly string[] Locations = new[]
    {
        "London", "Paris", "New York", "Texas", "Tokio", "Hong Kong", "Singapore", "Colambo", "New Delhi", "Bengaluru"
    };
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherAsync()
        {
            await Task.Yield();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Location = Locations[Random.Shared.Next(Summaries.Length)]
            })
       .ToArray();
        }



        public async Task<WeatherForecast> GetWeatherOnLocation(string location)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            WeatherForecast weather = new WeatherForecast();
            try
            {
                var apiURL = StaticConfigurationManager.AppSetting["weatherApi:weatherApiURL"] + StaticConfigurationManager.AppSetting["weatherApi:weatherApiKey"]
                    + "&q=" + location;

                weather = await GetWeatherData(apiURL);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
           
            return await Task.FromResult(weather);
        }

        public async Task<WeatherForecast> GetWeatherData(string apiURL)
        {
            WeatherForecast weather = new WeatherForecast();
            using HttpResponseMessage response = await _httpClient.GetAsync(apiURL);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<JToken>(responseBody);
            var interResult = new FetchResult<KeyValuePair<string, JToken>>(result);
            weather = new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                TemperatureC = (double)((JValue)interResult.result["current"]["temp_c"]).Value,
                Summary = ((JValue)interResult.result["current"]["condition"]["text"]).Value as string,
                Location = ((JValue)interResult.result["location"]["name"]).Value as string
            };
            return weather;
        }
    }
}

