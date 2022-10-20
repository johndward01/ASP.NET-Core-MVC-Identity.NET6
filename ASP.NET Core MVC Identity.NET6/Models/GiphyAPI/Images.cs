using ASP.NET_Core_Identity_Demo.Models.GiphyAPI;

namespace ASP.NET_Core_Identity_Demo.GiphyAPI
{
    public class Images
    {
        public Original original { get; set; }
        public Downsized downsized { get; set; }
        public Downsized_large downsized_large { get; set; }
        public Downsized_medium downsized_medium { get; set; }
        public Downsized_small downsized_small { get; set; }
        public Downsized_still downsized_still { get; set; }
        public Fixed_height fixed_height { get; set; }
        public Fixed_height_downsampled fixed_height_downsampled { get; set; }
        public Fixed_height_small fixed_height_small { get; set; }
        public Fixed_height_small_still fixed_height_small_still { get; set; }
        public Fixed_height_still fixed_height_still { get; set; }
        public Fixed_width fixed_width { get; set; }
        public Fixed_width_downsampled fixed_width_downsampled { get; set; }
        public Fixed_width_small fixed_width_small { get; set; }
        public Fixed_width_small_still fixed_width_small_still { get; set; }
        public Fixed_width_still fixed_width_still { get; set; }
        public Looping looping { get; set; }
        public Original_still original_still { get; set; }
        public Original_mp4 original_mp4 { get; set; }
        public Preview preview { get; set; }
        public Preview_gif preview_gif { get; set; }
        public Preview_webp preview_webp { get; set; }
        public _480w_still _480w_still { get; set; }
}
}

