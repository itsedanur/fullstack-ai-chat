using System;

namespace backend.SentimentApi.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Nick { get; set; } = "guest";
        public string Text { get; set; } = "";
        public string Sentiment { get; set; } = "UNKNOWN";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
