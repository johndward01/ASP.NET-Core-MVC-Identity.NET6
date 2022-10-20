using ASP.NET_Core_Identity_Demo.Models.GiphyAPI;

namespace ASP.NET_Core_Identity_Demo.GiphyAPI
{
    public class DataItem
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Url { get; set; }
        public string Slug { get; set; }
        public string BitlyGifUrl { get; set; }
        public string BitlyUrl { get; set; }
        public string EmbedUrl { get; set; }
        public string Username { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public string Rating { get; set; }
        public string ContentUrl { get; set; }
        public string SourceTld { get; set; }
        public string SourcePostUrl { get; set; }
        public int IsSticker { get; set; }
        public string ImportDatetime { get; set; }
        public string TrendingDatetime { get; set; }
        public Images Images { get; set; }
        public string AnalyticsResponsePayload { get; set; }
        public Analytics Analytics { get; set; }
    }
}





