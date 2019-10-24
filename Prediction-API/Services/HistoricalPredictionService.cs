using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;
using Prediction_API.Models;
using Prediction_API.Constants;

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
                return new NpgsqlConnection(this.configuration.GetConnectionString("PredictionStore-Development"));
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
                IEnumerable<decimal> prediction = await connection.QueryAsync<decimal>(
                    StoredProcedureConstants.GetPrediction,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        symbol = new DbString()
                        {
                            Value = tickerSymbol,
                            IsFixedLength = true,
                            Length = 6,
                            IsAnsi = true
                        },
                        prediction_date = dateTime
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
                    StoredProcedureConstants.GetPredictionsInRange,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        symbol = new DbString()
                        {
                            Value = tickerSymbol,
                            IsFixedLength = true,
                            Length = 6,
                            IsAnsi = true
                        },
                        start_date = start,
                        end_date = end
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
                // This method should return the number of affected rows, but it doesn't
                await connection.ExecuteAsync(
                    StoredProcedureConstants.AddPrediction,
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        symbol = new DbString()
                        {
                            Value = prediction.Symbol,
                            IsFixedLength = true,
                            Length = 6,
                            IsAnsi = true
                        },
                        prediction_date = prediction.Date,
                        price = prediction.Price
                    }
                );

                return prediction;
            }
        }
    }
}
