using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Prediction_API.Models;
using Prediction_API.Services;
using Prediction_API.Calculators;
using Prediction_API.Constants;

namespace Prediction_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PredictionController : Controller
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
        public async Task<IActionResult> Predict([FromBody] string ticker, [FromBody] string interval, [FromBody] string date)
        {
            // Convert the string date to a DateTime object
            DateTime dateTime = Convert.ToDateTime(date);

            try
            {
                // Check if this prediction has already been made -- if so return historical result
                decimal predictedPrice = await this.historicalPredictionService.GetPrediction(ticker, dateTime);

                return Json(new Prediction()
                {
                    Ticker = ticker,
                    Date = dateTime,
                    Price = predictedPrice
                });
            }
            catch (InvalidOperationException)
            {
                try
                {
                    // There is no price for this price -- calculate the prediction and insert it into the prediction table
                    List<StockTicker> tickers = await this.stockTickerService.GetStockTickersAsync(ticker, interval);

                    // Have the data from the stock service -- now run 20 day moving averages and regression on close prices
                    List<decimal> closePrices = tickers.Select(ticker => ticker.Close).ToList();
                    SimpleMovingAverageCalculator simpleMovingAverageCalculator = new SimpleMovingAverageCalculator();
                    WeightedMovingAverageCalculator weightedMovingAverageCalculator = new WeightedMovingAverageCalculator();

                    // Do the calculations for the moving averages
                    List<decimal> simpleMovingAverages = simpleMovingAverageCalculator.Calculate(closePrices, CalculationConstants.DEFAULT_MOVING_AVERAGE_PERIOD);
                    List<decimal> weightedMovingAverages = weightedMovingAverageCalculator.Calculate(closePrices, CalculationConstants.DEFAULT_MOVING_AVERAGE_PERIOD);

                    List<SimpleLinearRegressionCalculator> linearRegressionCalculators = new List<SimpleLinearRegressionCalculator>()
                    {
                        new SimpleLinearRegressionCalculator(closePrices),
                        new SimpleLinearRegressionCalculator(simpleMovingAverages),
                        new SimpleLinearRegressionCalculator(weightedMovingAverages)
                    };

                    decimal regressionSumForDate = 0;
                    foreach (SimpleLinearRegressionCalculator regressionCalculator in linearRegressionCalculators)
                    {
                        // Calculate the regression at this index (date) -- how to translate the date passed in into index??
                        regressionSumForDate += regressionCalculator.Calculate();
                    }

                    // Insert this prediction into the historical prediction service
                    Prediction prediction = new Prediction()
                    {
                        Ticker = ticker,
                        Date = dateTime,
                        Price = regressionSumForDate / linearRegressionCalculators.Count
                    };
                    
                    await this.historicalPredictionService.AddPrediction(prediction);

                    return Json(prediction);
                }
                catch (ApplicationException ex)
                {
                    Console.WriteLine(ex.Message);

                    // Return some error code??
                    return StatusCode(500);
                }
            }
        }
    }
}
