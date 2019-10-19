using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction_API.Models
{
    public class Prediction
    {
        public DateTime Date { get; set; }
        public decimal price { get; set; }
    }
}
