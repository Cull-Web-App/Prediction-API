using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Prediction_API.Models;

namespace Prediction_API.Services
{
    public class StockTickerService : IStockTickerService
    {
        // Keep the injected HTTP client in this service -- should be the typed client for this service!
        private readonly HttpClient httpClient;

        public StockTickerService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<StockTicker>> GetStockTickersAsync()
        {
            // Use the typed client to request from FinancialDataAPI the stock data for this time frame!
        }
    }
}
