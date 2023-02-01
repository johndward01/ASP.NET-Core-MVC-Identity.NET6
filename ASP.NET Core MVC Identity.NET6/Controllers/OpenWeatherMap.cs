using ASP.NET_Core_Identity_Demo.Models.Weather;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace ASP.NET_Core_Identity_Demo.Controllers
{
    public class OpenWeatherMap : Controller
    {
        private readonly IDbConnection _connection;

        public OpenWeatherMap(IDbConnection connection)
        {
            _connection = connection;
        }

        public IActionResult Index()
        {
            var weather = new Root();
            return View(weather);
        }

        [HttpPost]
        public IActionResult GetWeather(Root rootWeather)
        {
            try
            {
                var client = new HttpClient();
                var url = $"https://api.openweathermap.org/data/2.5/forecast?" +
                               $"q={rootWeather.City.Name}" +
                               $"&appid={rootWeather.API_Key}" +
                               $"&units=imperial";

                var jsonResponse = client.GetStringAsync(url).Result;

                var jObj = JObject.Parse(jsonResponse);


                rootWeather = JsonConvert.DeserializeObject<Root>(jsonResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Index");
            }
            

            return View(rootWeather);
        }

    }
}
