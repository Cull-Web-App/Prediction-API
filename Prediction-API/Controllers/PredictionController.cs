using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
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
        private readonly IQuoteService quoteService;
        private readonly IHistoricalPredictionService historicalPredictionService;

        public PredictionController(IQuoteService quoteService, IHistoricalPredictionService historicalPredictionService)
        {
            this.quoteService = quoteService;
            this.historicalPredictionService = historicalPredictionService;
        }

        [HttpPost]
        public async Task<IActionResult> Predict([FromBody] PredictionBodyWrapper predictionBodyWrapper)
        {
            // Extract the body properties
            string symbol = predictionBodyWrapper.Symbol;
            string interval = predictionBodyWrapper.Interval;
            string date = predictionBodyWrapper.Date;

            // Add logging
            Console.WriteLine(string.Format("Received request for prediction for {0}'s price on date {1}", symbol, date));

            // Convert the string date to a DateTime object
            DateTime dateTime = Convert.ToDateTime(date);
            TimeSpan timeToPredictionDate = dateTime - DateTime.Now;
            double daysToPredictionDate = timeToPredictionDate.TotalDays;

            // Probably should do some sort of check here to see if the date has already passed, because then actuals can be retrieved
            if (daysToPredictionDate <= 0)
            {
                Console.WriteLine("The date sent has already passed");

                // What to do in this case?? -- probably just retrieve the prediction or request the actual from financial data
                return Json(new Prediction()
                {
                    Symbol = symbol,
                    Date = dateTime,
                    Price = (await this.quoteService.GetQuotesAsync(symbol, interval)).FirstOrDefault().Close
                });
            }

            try
            {
                // Check if this prediction has already been made -- if so return historical result
                Console.WriteLine("Checking if prediction is already recorded");
                decimal predictedPrice = await this.historicalPredictionService.GetPredictionAsync(symbol, dateTime);

                return Json(new Prediction()
                {
                    Symbol = symbol,
                    Date = dateTime,
                    Price = predictedPrice
                });
            }
            catch (InvalidOperationException)
            {
                // In this case there was no result for the prediction in the DB -- run the prediction process
                Console.WriteLine("Prediction wasn't recorded, creating a new prediction");
                // There is no price for this price -- calculate the prediction and insert it into the prediction table
                // List<Quote> quotes = await this.quoteService.GetQuotesAsync(symbol, interval);

                List<Quote> quotes = new List<Quote>();
                for (int i = 0; i < 10000; i++)
                {
                    quotes.Add(new Quote()
                    {
                        Close = i
                    });
                }

                // Have the data from the stock service -- now run 20 day moving averages and regression on close prices
                List<decimal> closePrices = quotes.Select(quote => quote.Close).ToList();
                SimpleMovingAverageCalculator simpleMovingAverageCalculator = new SimpleMovingAverageCalculator();
                WeightedMovingAverageCalculator weightedMovingAverageCalculator = new WeightedMovingAverageCalculator();

                // Do the calculations for the moving averages -- run on separate threads using TPL
                Task<List<decimal>> simpleMovingAverageTask = Task.Run<List<decimal>>(() => simpleMovingAverageCalculator.Calculate(closePrices, CalculationConstants.DEFAULT_MOVING_AVERAGE_PERIOD));
                Task<List<decimal>> weightedMovingAverageTask = Task.Run<List<decimal>>(() => weightedMovingAverageCalculator.Calculate(closePrices, CalculationConstants.DEFAULT_MOVING_AVERAGE_PERIOD));

                // Wait for the threads to finish
                await Task.WhenAll(simpleMovingAverageTask, weightedMovingAverageTask);

                // Pull out the results from the tasks
                List<decimal> simpleMovingAverages = await simpleMovingAverageTask;
                List<decimal> weightedMovingAverages = await weightedMovingAverageTask;

                // Should this list construction also be multithreaded??
                List<SimpleLinearRegressionCalculator> linearRegressionCalculators = new List<SimpleLinearRegressionCalculator>()
                {
                    new SimpleLinearRegressionCalculator(closePrices),
                    new SimpleLinearRegressionCalculator(simpleMovingAverages),
                    new SimpleLinearRegressionCalculator(weightedMovingAverages)
                };

                decimal regressionSumForDate = 0;
                foreach (SimpleLinearRegressionCalculator regressionCalculator in linearRegressionCalculators)
                {
                    // Calculate the regression at this index (date) -- this currently always assumes you're in days!!
                    regressionSumForDate += regressionCalculator.Calculate(quotes.Count + Convert.ToInt32(daysToPredictionDate) - 1);
                }

                // Insert this prediction into the historical prediction service
                Prediction prediction = new Prediction()
                {
                    Symbol = symbol,
                    Date = dateTime,
                    Price = regressionSumForDate / linearRegressionCalculators.Count
                };

                Console.WriteLine(string.Format("New prediction created: Symbol: {0}, Date: {1}, Price: {2}", prediction.Symbol, prediction.Date, prediction.Price));
                    
                await this.historicalPredictionService.AddPredictionAsync(prediction);

                return Json(prediction);
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);

                // Return some error code??
                return StatusCode(500);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("There was an error accessing an HTTP resource.");

                return StatusCode(500);
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("There was an error trying to connect to the SQL database");

                return StatusCode(500);
            }
        }
    }
}
