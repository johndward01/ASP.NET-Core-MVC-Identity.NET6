using ASP.NET_Core_Identity_Demo.GiphyAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading;

namespace ASP.NET_Core_Identity_Demo.Controllers
{
    public class GiphyController : Controller
    {
        public IActionResult Index()
        {            
            var client = new HttpClient();
            var url = "https://api.giphy.com/v1/gifs/search?api_key=IVv33It9Q4JM0mn8AfKlwxfLwkkY28yx&q=cat+meme&limit=25&offset=0&rating=pg-13&lang=en";
            var response = client.GetStringAsync(url).Result;
            var root = JsonConvert.DeserializeObject<GiphyAPI_Root>(response);
            return View(root);
        }
    }
}
