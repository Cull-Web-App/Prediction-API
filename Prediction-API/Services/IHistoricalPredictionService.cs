using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prediction_API.Models;

namespace Prediction_API.Services
{
    public interface IHistoricalPredictionService
    {
        public Task<decimal> GetPrediction(string tickerSymbol, DateTime dateTime);
        public Task<List<Prediction>> GetPredictionsInRange(string tickerSymbol, DateTime start, DateTime end);
    }
}
