using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using Prediction_API.Models;

namespace Prediction_API.Services
{
    public class HistoricalPredictionService : IHistoricalPredictionService
    {
        // Configuration pulled from appSettings (or web config??) for the DB connection
        private readonly IConfiguration configuration;

        private IDbConnection Connection
        {
            get
            {
                // TODO: Use constant for this connection string
                return new SqlConnection(this.configuration.GetConnectionString("PredictionStore"));
            }
        }

        public HistoricalPredictionService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<decimal> GetPredictionAsync(string tickerSymbol, DateTime dateTime)
        {
            // Get the historical prediction from the RDS database for this index on this date
            // Connections are disposable!
            using (IDbConnection connection = this.Connection)
            {
                // TODO: Replace spName with the sproc name in Aurora Serverless
                IEnumerable<decimal> prediction = await connection.QueryAsync<decimal>(
                    "GetPrediction",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Symbol = tickerSymbol,
                        Prediction_Date = dateTime
                    }
                );
                return prediction.First();
            }
        }

        public async Task<List<Prediction>> GetPredictionsInRangeAsync(string tickerSymbol, DateTime start, DateTime end)
        {
            // Get all the predictions for a ticker in the defined range
            using (IDbConnection connection = this.Connection)
            {
                IEnumerable<Prediction> predictions = await connection.QueryAsync<Prediction>(
                    "GetPredictionsInRange",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Symbol = tickerSymbol,
                        Start_Date = start,
                        End_Date = end
                    }
                );
                return predictions.ToList();
            }
        }

        public async Task<Prediction> AddPredictionAsync(Prediction prediction)
        {
            // Add the new prediction for this ticker to the RDS DB -- can't use Dapper on updates!
            using (IDbConnection connection = this.Connection)
            {
                int numAffectedRows = await connection.ExecuteAsync(
                    "spName",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Symbol = prediction.Ticker,
                        Prediction_Date = prediction.Date,
                        Price = prediction.Price
                    }
                );

                if (numAffectedRows != 1)
                {
                    throw new ApplicationException(string.Format("Failed to add prediction for ticker {0} for date {1} of price {2}", prediction.Ticker, prediction.Date, prediction.Price));
                }

                return prediction;
            }
        }
    }
}
