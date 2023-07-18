using ASP.NET_Core_Identity_Demo.GiphyAPI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASP.NET_Core_Identity_Demo.Controllers
{
    public class GiphyController : Controller
    {
        private readonly IConfiguration _configuration;

        public GiphyController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            string apiKey = _configuration.GetConnectionString("Giphy");
            var client = new HttpClient();
            var url = $"https://api.giphy.com/v1/gifs/search?api_key={apiKey}&q=cat+meme&limit=25&offset=0&rating=pg-13&lang=en";
            var response = client.GetStringAsync(url).Result;
            var root = JsonConvert.DeserializeObject<GiphyAPI_Root>(response);
            return View(root);
        }
    }
}
