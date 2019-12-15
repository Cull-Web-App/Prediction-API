using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prediction_API.Models;

namespace Prediction_API.Services
{
    public interface IHistoricalPredictionService
    {
        public Task<decimal> GetPredictionAsync(string symbol, DateTime dateTime);
        public Task<List<Prediction>> GetPredictionsInRangeAsync(string symbol, DateTime start, DateTime end);
        public Task<Prediction> AddPredictionAsync(Prediction prediction);
    }
}
