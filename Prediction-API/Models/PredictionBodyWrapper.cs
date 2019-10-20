using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction_API.Models
{
    public class PredictionBodyWrapper
    {
        public string TickerSymbol { get; set; }
        public string Interval { get; set; }
        public string Date { get; set; }
    }
}
