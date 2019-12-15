using System;

namespace Prediction_API.Models
{
    public class Quote
    {
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public DateTime Date { get; set; }
        // This is the time of the day if this is an intra-day query -- not going to be the case in this service
        public string? Time { get; set; }
    }
}
