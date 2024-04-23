using Newtonsoft.Json;

namespace HackerNewsWeb.Models
{
    public class NewsItem
    {
        public long Id { get; set; }
        public string? By {  get; set; }
        public int Descendants { get; set; }
        public long[] Kids { get; set; }
        public int Score { get; set; }
        public long Time { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public string? Url { get; set; }
    }
}
