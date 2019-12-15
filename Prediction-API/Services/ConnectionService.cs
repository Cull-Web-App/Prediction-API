using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prediction_API.Services
{
    public class ConnectionService : IConnectionService
    {
        public string CreateDBConnectionString(string dbUsername, string dbPassword, string dbServer, string dbName, int portNum)
        {
            return string.Format(
                "Userid={0};Password={1};Server={2};Port={3};Database={4};Pooling=true;MinPoolSize=1;MaxPoolSize=20;Timeout=30",
                dbUsername,
                dbPassword,
                dbServer,
                portNum,
                dbName
            );
        }
    }
}
