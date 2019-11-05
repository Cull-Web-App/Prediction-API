using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Prediction_API.Models
{
    public class Prediction
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public decimal AccuracyScore { get; set; }
        public DateTime PredictedOn { get; set; }
    }
}
