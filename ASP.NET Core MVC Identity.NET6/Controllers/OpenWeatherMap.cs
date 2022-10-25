using ASP.NET_Core_Identity_Demo.Models.Weather;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;

namespace ASP.NET_Core_Identity_Demo.Controllers
{
    public class OpenWeatherMap : Controller
    {
        public IActionResult Index()
        {
            var weather = new Root();
            return View(weather);
        }

        [HttpPost]
        public IActionResult GetWeather(Root rootWeather)
        {               
            var client = new HttpClient();
            var url = $"https://api.openweathermap.org/data/2.5/forecast?" +
                           $"q={rootWeather.City.Name}" +
                           $"&appid={rootWeather.API_Key}" +
                           $"&units=imperial";

            var jsonResponse = client.GetStringAsync(url).Result;
            rootWeather = JsonConvert.DeserializeObject<Root>(jsonResponse);

            return View(rootWeather);
        }

    }
}
