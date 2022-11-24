using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Drinks4Us.Models;
using Newtonsoft.Json;
using WeatherAPI.NET;
using WeatherAPI.NET.Entities;

namespace Drinks4Us.Services
{
    public class WeatherApiService
    {
        public WeatherAPIClient WeatherApiClient = new WeatherAPIClient("c4e96e573c904050872120953212212");


        private List<WeatherCondition> WeatherConditions { get; }

        private const string IconUrlBase = "https://cdn.weatherapi.com/weather/128x128/{0}/{1}.png";

        public WeatherApiService()
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };
            using var httpClient = new HttpClient(clientHandler);
            var json = httpClient.GetStringAsync("https://www.weatherapi.com/docs/weather_conditions.json").Result;

            WeatherConditions = JsonConvert.DeserializeObject<List<WeatherCondition>>(json);
        }

        public async Task<CurrentWeatherCondition> GetCurrentWeatherCondition()
        {
            var requestEntity = new RealtimeRequestEntity()
                .WithAutoIP()
                .WithLanguage("nl");

            var result = await WeatherApiClient.Realtime.GetCurrentAsync(requestEntity).ConfigureAwait(false);
            var conditionCode = result.Current.Condition.Code;
            var weatherCondition = WeatherConditions.FirstOrDefault(weatherCondition1 => weatherCondition1.code == conditionCode);

            //Console.WriteLine(WeatherConditions.Count);

            if (weatherCondition != null)
            {
                return new CurrentWeatherCondition
                {
                    IconUrl = string.Format(IconUrlBase, result.Current.IsDay ? "day" : "night", weatherCondition.icon),
                    Temperature = result.Current.TemperatureC
                };
            }

            throw new Exception("WeatherCondition is null");
        }
    }
}
