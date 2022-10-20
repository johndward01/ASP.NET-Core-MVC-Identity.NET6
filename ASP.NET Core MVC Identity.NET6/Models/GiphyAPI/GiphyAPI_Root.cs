using ASP.NET_Core_Identity_Demo.Models.GiphyAPI;
using System.Collections.Generic;

namespace ASP.NET_Core_Identity_Demo.GiphyAPI
{
    public class GiphyAPI_Root
    {
        public List<DataItem> data { get; set; }
        public Pagination pagination { get; set; }
        public Meta meta { get; set; }
    }

}





