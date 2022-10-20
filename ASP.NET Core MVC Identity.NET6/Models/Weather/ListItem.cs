using System.Collections.Generic;

namespace ASP.NET_Core_Identity_Demo.Models.Weather
{
    public class ListItem
    {
        public int Dt { get; set; }
        public Main Main { get; set; }
        public List<WeatherItem> Weather { get; set; }
        public Clouds Clouds { get; set; }
        public Wind Wind { get; set; }
        public int Visibility { get; set; }
        public double Pop { get; set; }
        public Sys Sys { get; set; }
        public string Dt_txt { get; set; }
    }

}
