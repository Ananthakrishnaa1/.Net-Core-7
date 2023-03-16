using Main.Service.Models;
using Main.Service.Utility;
using Newtonsoft.Json;
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
        static readonly HttpClient client = new HttpClient();

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
                using HttpResponseMessage response = await client.GetAsync(apiURL);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);
                var result = JsonConvert.DeserializeObject(responseBody);
                weather = new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                    Location = Locations[Random.Shared.Next(Summaries.Length)]
                };
                
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

           
            return await Task.FromResult(weather);
        }
    }
}

