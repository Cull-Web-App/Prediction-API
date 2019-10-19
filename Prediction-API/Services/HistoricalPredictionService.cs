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
                return new SqlConnection(this.configuration.GetConnectionString(""));
            }
        }

        public HistoricalPredictionService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<decimal> GetPrediction(string tickerSymbol, DateTime dateTime)
        {
            // Get the historical prediction from the RDS database for this index on this date
            // Connections are disposable!
            using (IDbConnection connection = this.Connection)
            {

            }
        }

        public async Task<List<Prediction>> GetPredictionsInRange(string tickerSymbol, DateTime start, DateTime end)
        {
            // Get all the predictions for a ticker in the defined range
            using (IDbConnection connection = this.Connection)
            {

            }
        }
    }
}
