using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_Identity_Demo.Models.Weather
{
    public class Root
    {
        public string? Cod { get; set; }
        public int Message { get; set; }
        public int Cnt { get; set; }
        public List<ListItem> List { get; set; }
        public City City { get; set; }
        public string API_Key { get; set; }
    }

}



