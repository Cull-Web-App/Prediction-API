using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prediction_API.Models;
using Prediction_API.Services;
using Prediction_API.Calculators;

namespace Prediction_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly IStockTickerService stockTickerService;
        private readonly IHistoricalPredictionService historicalPredictionService;

        public PredictionController(IStockTickerService stockTickerService, IHistoricalPredictionService historicalPredictionService)
        {
            this.stockTickerService = stockTickerService;
            this.historicalPredictionService = historicalPredictionService;
        }

        // POST: Prediction -- Create a new prediction and update it in the DB
        [HttpPost]
        public async void Predict([FromBody] string ticker, [FromBody] string interval)
        {
            try
            {
                // Get the stock data for this ticker and interval
                List<StockTicker> tickers = await this.stockTickerService.GetStockTickersAsync(ticker, interval);

                // Calculate the moving average for 2 x 20 x interval
                
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);

                // Return some error code??
            }
        }
    }
}
