using System;

namespace Prediction_API.Models
{
    public class StockTicker
    {
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
    }
}
