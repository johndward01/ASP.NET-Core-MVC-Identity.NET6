using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_Identity_Demo.Models.Weather
{
    public class City
    {
        public int Id { get; set; }
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public Coord Coord { get; set; }
        public string Country { get; set; }
        public int Population { get; set; }
        public int Timezone { get; set; }
        public int Sunrise { get; set; }
        public int Sunset { get; set; }
    }

}
