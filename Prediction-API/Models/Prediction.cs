using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction_API.Models
{
    public class Prediction
    {
        public string Ticker { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}
