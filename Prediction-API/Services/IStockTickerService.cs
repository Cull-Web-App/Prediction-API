using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prediction_API.Models;

namespace Prediction_API.Services
{
    public interface IStockTickerService
    {
        public Task<List<StockTicker>> GetStockTickersAsync(string ticker, string interval);
    }
}
